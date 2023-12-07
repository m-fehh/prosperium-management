using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prosperium.Management.ExternalServices.Pluggy;
using Prosperium.Management.OpenAPI.V1.Accounts.Dto;
using Prosperium.Management.OpenAPI.V1.Banks;
using Prosperium.Management.OpenAPI.V1.Banks.Dtos;
using Prosperium.Management.OpenAPI.V1.Categories;
using Prosperium.Management.OpenAPI.V1.CreditCards;
using Prosperium.Management.OpenAPI.V1.Customers;
using Prosperium.Management.OpenAPI.V1.Transactions;
using Prosperium.Management.OpenAPI.V1.Transactions.Dto;
using Prosperium.Management.OriginDestinations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using static Prosperium.Management.OpenAPI.V1.Accounts.AccountConsts;

namespace Prosperium.Management.OpenAPI.V1.Accounts
{
    [Route("v1/accounts")]
    public class AccountAppService : ManagementAppServiceBase, IAccountAppService
    {
        private readonly IRepository<AccountFinancial, long> _accountFinancialRepository;
        private readonly IRepository<Bank, long> _banksRepository;
        private readonly IRepository<Category, long> _categoryRepository;
        private readonly ITransactionAppService _transactionAppService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly PluggyManager _pluggyManager;
        private readonly ICreditCardAppService _creditCardAppService;
        private readonly ICustomerAppService _customerAppService;

        public AccountAppService(IRepository<AccountFinancial, long> accountFinancialRepository, IRepository<Bank, long> banksRepository, IRepository<Category, long> categoryRepository, ITransactionAppService transactionAppService, IUnitOfWorkManager unitOfWorkManager, PluggyManager pluggyManager, ICreditCardAppService creditCardAppService, ICustomerAppService customerAppService)
        {
            _accountFinancialRepository = accountFinancialRepository;
            _banksRepository = banksRepository;
            _categoryRepository = categoryRepository;
            _transactionAppService = transactionAppService;
            _unitOfWorkManager = unitOfWorkManager;
            _pluggyManager = pluggyManager;
            _creditCardAppService = creditCardAppService;
            _customerAppService = customerAppService;
        }

        [HttpGet]
        [Route("list-banks")]
        public async Task<List<BankDto>> GetAllListBanksAsync()
        {
            List<Bank> allBanks = await _banksRepository.GetAllListAsync();
            return ObjectMapper.Map<List<BankDto>>(allBanks);
        }

        [HttpGet]
        [Route("GetAccountById")]
        public async Task<AccountFinancialDto> GetAccountById(long id)
        {
            var account = await _accountFinancialRepository.FirstOrDefaultAsync(x => x.Id == id);
            return ObjectMapper.Map<AccountFinancialDto>(account);
        }

        [HttpGet]
        public async Task<List<AccountFinancialDto>> GetAllListAsync()
        {
            List<AccountFinancial> allAccounts = await _accountFinancialRepository.GetAll().Include(x => x.Bank).ToListAsync();
            return ObjectMapper.Map<List<AccountFinancialDto>>(allAccounts);
        }

        [HttpPost]
        public async Task CreateAsync(AccountFinancialDto input)
        {
            AccountFinancial account = ObjectMapper.Map<AccountFinancial>(input);

            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                account.Origin = AccountConsts.AccountOrigin.Manual;
                account.IsActive = true;
                await _accountFinancialRepository.InsertAndGetIdAsync(account);
                uow.Complete();
            }

            // Quando adicionado uma conta, deve constar o valor no extrato
            var extractCreated = new CreateTransactionDto
            {
                Date = DateTime.Now,
                Description = $"Saldo da conta: {input.AccountNickname}",
                ExpenseValue = input.BalanceAvailable,
                AccountId = account.Id,
                TransactionType = TransactionConsts.TransactionType.Saldo,
                PaymentTerm = TransactionConsts.PaymentTerms.Saldo,
                PaymentType = TransactionConsts.PaymentType.Saldo,
                CategoryId = _categoryRepository.FirstOrDefaultAsync(x => x.Name == "Saldo da Conta").Result.Id,
            };

            await _transactionAppService.CreateAsync(extractCreated);
        }

        [HttpPut]
        public async Task StatusChangeAccountAsync(long id, bool statusChange)
        {
            var account = await _accountFinancialRepository.FirstOrDefaultAsync(id);
            account.IsActive = statusChange;

            await _accountFinancialRepository.UpdateAsync(account);
        }

        [HttpDelete("id")]
        public async Task DeleteAsync(long id)
        {
            AccountFinancial searchAccount = await _accountFinancialRepository.GetAll()
                .Include(x => x.CreditCards)
                    .ThenInclude(x => x.Transactions)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (searchAccount.CreditCards.Count > 0)
            {
                foreach (var cards in searchAccount.CreditCards)
                {
                    await _creditCardAppService.DeleteAsync(cards.Id);
                }
            }

            if (searchAccount.Origin == AccountOrigin.Pluggy)
            {
                await _pluggyManager.PluggyItemDeleteAsync(searchAccount.PluggyItemId);
            }

            await _accountFinancialRepository.DeleteAsync(searchAccount);
        }

        #region Pluggy API 

        [HttpPost]
        [Route("PluggyCreateAccount")]
        public async Task PluggyCreateAccount(string itemId)
        {
            var accountsAlreadysaved = await GetAllListAsync();
            var account = await _pluggyManager.PluggyGetAccount(itemId);

            if (account.Total > 0)
            {
                long accountId = new long();

                #region Account creation   
                var accountsBanking = account.Results.Where(x => x.Type.ToUpper().Trim().Equals("BANK")).FirstOrDefault();
                if (accountsBanking != null)
                {
                    var (agencyNumber, accountNumber) = ExtractAgencyAndAccount(accountsBanking.Number);

                    var isItemAlreadySaved = accountsAlreadysaved.Any(x => x.PluggyItemId == accountsBanking.ItemId && x.PluggyAccountId == accountsBanking.Id);
                    if (!isItemAlreadySaved)
                    {
                        var bankCreated = await CreateBankFromPluggy(itemId);
                        var accountDto = new AccountFinancialDto
                        {
                            BankId = bankCreated.Id,
                            AccountNickname = bankCreated.BankName,
                            AgencyNumber = agencyNumber,
                            AccountNumber = accountNumber,
                            BalanceAvailable = accountsBanking.Balance,
                            AccountType = MapStringToAccountType(accountsBanking.Name),
                            MainAccount = false,
                            IsActive = true,
                            Origin = AccountConsts.AccountOrigin.Pluggy,
                            PluggyItemId = accountsBanking.ItemId,
                            PluggyAccountId = accountsBanking.Id
                        };

                        AccountFinancial accountToInsert = ObjectMapper.Map<AccountFinancial>(accountDto);

                        using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                        {
                            await _accountFinancialRepository.InsertAndGetIdAsync(accountToInsert);
                            accountId = accountToInsert.Id;
                            uow.Complete();
                        }

                        await _transactionAppService.CapturePluggyTransactionsAsync(accountsBanking.Id, null, null, false);
                    }
                }
                #endregion

                #region Credit card creation    
                var creditCard = account.Results.Where(x => x.Type.ToUpper().Trim().Equals("CREDIT")).FirstOrDefault();
                if (creditCard != null)
                {
                    await _creditCardAppService.PluggyCreateCreditCard(creditCard, accountId);

                }

                #endregion
                
                await _customerAppService.PluggyCreateCustomer(itemId);
            }
        }

        private async Task<(long Id, string BankName)> CreateBankFromPluggy(string itemId)
        {
            var bankPluggy = await _pluggyManager.PluggyGetItemId(itemId);
            var allBanksProsperium = await GetAllListBanksAsync();

            var isItemAlreadySaved = allBanksProsperium.Where(x => x.Name == bankPluggy.Connector.Name).FirstOrDefault();
            if (isItemAlreadySaved == null)
            {
                Bank newBank = new Bank
                {
                    Name = bankPluggy.Connector.Name,
                    Origin = AccountOrigin.Pluggy
                };

                using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                {
                    await _banksRepository.InsertAndGetIdAsync(newBank);
                    uow.Complete();
                    return (newBank.Id, newBank.Name);
                }
            }

            return (isItemAlreadySaved.Id, isItemAlreadySaved.Name);
        }

        private (string AgencyNumber, string AccountNumber) ExtractAgencyAndAccount(string apiString)
        {
            var regex = new Regex(@"(\d+)/(\d+)-(\d+)");
            var match = regex.Match(apiString);

            if (match.Success)
            {
                var agencyNumber = match.Groups[1].Value;
                var accountNumber = match.Groups[2].Value;

                return (agencyNumber, accountNumber);
            }

            throw new ArgumentException("Formato inválido para extração de agência e conta");
        }

        private AccountType MapStringToAccountType(string accountTypeName)
        {
            switch (accountTypeName.ToLower())
            {
                case "conta corrente":
                    return AccountType.Corrente;
                case "poupança":
                    return AccountType.Poupança;
                case "investimento":
                    return AccountType.Investimento;
                case "outros":
                    return AccountType.Outros;
                case "indefinido":
                    return AccountType.Indefinido;
                case "benefícios":
                    return AccountType.Benefícios;
                default:
                    throw new ArgumentException("Tipo de conta não reconhecido", nameof(accountTypeName));
            }
        }

        #endregion
    }
}
