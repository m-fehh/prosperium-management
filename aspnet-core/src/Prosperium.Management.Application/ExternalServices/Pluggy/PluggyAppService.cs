using static Prosperium.Management.OpenAPI.V1.Accounts.AccountConsts;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System;
using Prosperium.Management.OpenAPI.V1.Accounts;
using Abp.UI;
using Prosperium.Management.ExternalServices.Pluggy.Dtos;
using System.Linq;
using Prosperium.Management.OpenAPI.V1.CreditCards;
using Prosperium.Management.OpenAPI.V1.CreditCards.Dto;
using Prosperium.Management.OpenAPI.V1.Accounts.Dto;
using Prosperium.Management.OpenAPI.V1.Customers;
using Prosperium.Management.OpenAPI.V1.Transactions;
using Abp.Domain.Repositories;
using Prosperium.Management.OriginDestinations;
using System.Collections.Generic;
using Prosperium.Management.OpenAPI.V1.Transactions.Dto;
using static Prosperium.Management.OpenAPI.V1.Transactions.TransactionConsts;
using Prosperium.Management.OpenAPI.V1.Categories;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing;

namespace Prosperium.Management.ExternalServices.Pluggy
{
    public class PluggyAppService : ManagementAppServiceBase, IPluggyAppService
    {
        private readonly PluggyManager _pluggyManager;
        private readonly IAccountAppService _accountAppService;
        private readonly ICreditCardAppService _creditCardAppService;
        private readonly ICustomerAppService _customerAppService;
        private readonly ITransactionAppService _transactionAppService;
        private readonly ICategoryAppService _categoryAppService;
        private readonly IRepository<OriginDestination> _originDestinationRepository;

        public PluggyAppService(PluggyManager pluggyManager, IAccountAppService accountAppService, ICreditCardAppService creditCardAppService, ICustomerAppService customerAppService, ITransactionAppService transactionAppService, ICategoryAppService categoryAppService, IRepository<OriginDestination> originDestinationRepository)
        {
            _pluggyManager = pluggyManager;
            _accountAppService = accountAppService;
            _creditCardAppService = creditCardAppService;
            _customerAppService = customerAppService;
            _transactionAppService = transactionAppService;
            _categoryAppService = categoryAppService;
            _originDestinationRepository = originDestinationRepository;
        }

        public async Task<List<string>> PluggyCreateAsync(string itemId)
        {
            var pluggyAccounts = await _pluggyManager.PluggyGetAccountAsync(itemId);
            if (pluggyAccounts == null || pluggyAccounts.Total == 0)
            {
                throw new UserFriendlyException("Erro ao criar a conta, por favor, acione a equipe de suporte.");
            }

            Dictionary<string, int> accountsCount = new Dictionary<string, int>();

            foreach (var item in pluggyAccounts.Results)
            {
                await (item.Type == "CREDIT" ? CreateCreditCardAsync(item) : CreateAccountAsync(item));

                // Atualiza a contagem do tipo de conta
                if (accountsCount.ContainsKey(item.Type))
                {
                    accountsCount[item.Type]++;
                }
                else
                {
                    accountsCount[item.Type] = 1;
                }

                await CapturePluggyTransactionsAsync(item.Id, null, null, (item.Type == "CREDIT"));
            }

            await _customerAppService.PluggyCreateCustomer(itemId);
            List<string> accountsCreated = accountsCount.Select(kv => $"{kv.Value} {kv.Key}").ToList();

            return accountsCreated;
        }

        public async Task CapturePluggyTransactionsAsync(string pluggyAccountOrCardId, DateTime? dateInitial, DateTime? dateEnd, bool isCreditCard)
        {
            ResultPluggyTransactions pluggyTransactions = await _pluggyManager.PluggyGetTransactionsAsync(pluggyAccountOrCardId, dateInitial, dateEnd);
            if (pluggyTransactions.Total > 0)
            {
                var originDestinations = await _originDestinationRepository.GetAllListAsync();
                var transactionsAlreadySaved = await _transactionAppService.GetAllListAsync();

                List<Transaction> insertPendingTransactions = new List<Transaction>();
                var transactionsWithErrors = new List<string>();

                foreach (var item in pluggyTransactions.Results)
                {
                    try
                    {
                        var isItemAlreadySaved = transactionsAlreadySaved.Any(x => x.PluggyTransactionId == item.Id);
                        if (!isItemAlreadySaved)
                        {
                            TransactionDto transactionDto = new TransactionDto();
                            if (isCreditCard)
                            {
                                transactionDto.TransactionType = TransactionType.Gastos;
                                transactionDto.ExpenseValue = (item.Amount > 0) ? Convert.ToDecimal(item.Amount * -1) : Convert.ToDecimal(item.Amount);
                                transactionDto.Description = item.Description;
                                transactionDto.CategoryId = (item.CategoryId == "29") ? (await _categoryAppService.GetAllListAsync()).Where(x => x.Name == "Fatura Mensal").Select(x => x.Id).FirstOrDefault() : (!string.IsNullOrEmpty(item.CategoryId)) ? Convert.ToInt64(originDestinations.Where(x => x.OriginValueId == item.CategoryId).Select(x => x.DestinationValueId).FirstOrDefault()) : (await _categoryAppService.GetAllListAsync()).Where(x => x.Name == "Outros").Select(x => x.Id).FirstOrDefault();
                                transactionDto.PaymentType = PaymentType.Crédito;
                                transactionDto.PaymentTerm = (item.CreditCardMetadata?.TotalInstallments > 1) ? PaymentTerms.Parcelado : PaymentTerms.Imediatamente;
                                transactionDto.Installments = (item.CreditCardMetadata?.TotalInstallments > 1) ? item.CreditCardMetadata.TotalInstallments : 1;
                                transactionDto.CurrentInstallment = (item.CreditCardMetadata?.TotalInstallments > 1) ? $"{item.CreditCardMetadata.InstallmentNumber}/{item.CreditCardMetadata.TotalInstallments}" : "1/1";
                                transactionDto.CreditCardId = (await _creditCardAppService.GetAllListAsync()).Where(x => x.PluggyCreditCardId == pluggyAccountOrCardId).Select(x => x.Id).FirstOrDefault();
                                transactionDto.Date = item.Date;
                                transactionDto.Origin = AccountOrigin.Pluggy;
                                transactionDto.PluggyTransactionId = item.Id;
                            }
                            else
                            {
                                transactionDto.TransactionType = (item.Amount > 0) ? TransactionType.Ganhos : TransactionType.Gastos;
                                transactionDto.ExpenseValue = Convert.ToDecimal(item.Amount);
                                transactionDto.Description = item.Description;
                                transactionDto.CategoryId = (!string.IsNullOrEmpty(item.CategoryId)) ? Convert.ToInt64(originDestinations.Where(x => x.OriginValueId == item.CategoryId).Select(x => x.DestinationValueId).FirstOrDefault()) : (await _categoryAppService.GetAllListAsync()).Where(x => x.Name == "Outros").Select(x => x.Id).FirstOrDefault();
                                transactionDto.PaymentType = (item.Type == "CREDIT") ? PaymentType.Crédito : PaymentType.Débito;
                                transactionDto.PaymentTerm = PaymentTerms.Imediatamente;
                                transactionDto.AccountId = (await _accountAppService.GetAllListAsync()).Where(x => x.PluggyAccountId == pluggyAccountOrCardId).Select(x => x.Id).FirstOrDefault(); ;
                                transactionDto.Installments = 1;
                                transactionDto.CurrentInstallment = "1/1";
                                transactionDto.Date = item.Date;
                                transactionDto.Origin = AccountOrigin.Pluggy;
                                transactionDto.PluggyTransactionId = item.Id;
                            }

                            Transaction transactionToInsert = ObjectMapper.Map<Transaction>(transactionDto);
                            insertPendingTransactions.Add(transactionToInsert);
                        }
                    }
                    catch (Exception)
                    {
                        transactionsWithErrors.Add(item.Id);
                    }
                }

                if (transactionsWithErrors.Count > 0)
                {
                    Logger.Error($"Transações com erros: {string.Join(", ", transactionsWithErrors)} - accountOrCardPluggyId: {pluggyAccountOrCardId}");
                }

                await _transactionAppService.CreateAtomicTransactionAsync(insertPendingTransactions);
            }
        }

        private async Task CreateAccountAsync(PluggyAccount input)
        {
            var (bankId, bankName) = await BankFromPluggy(input.ItemId);
            var (agencyNumber, accountNumber) = ExtractAgencyAndAccount(input.BankData.TransferNumber);

            var accountAlreadysaved = (await _accountAppService.GetAllListAsync()).Any(x => x.PluggyItemId == input.ItemId && x.PluggyAccountId == input.Id);
            if (!accountAlreadysaved)
            {
                AccountFinancialDto accountFinancialDto = new()
                {
                    BankId = bankId,
                    AccountNickname = bankName,
                    AgencyNumber = agencyNumber,
                    AccountNumber = accountNumber,
                    BalanceAvailable = input.Balance.Value,
                    AccountType = input.Subtype == "CHECKING_ACCOUNT" ? AccountType.Corrente : AccountType.Poupança,
                    MainAccount = false,
                    IsActive = true,
                    Origin = AccountOrigin.Pluggy,
                    PluggyItemId = input.ItemId,
                    PluggyAccountId = input.Id,
                };

                await _accountAppService.CreateAsync(accountFinancialDto);
            }
        }

        private async Task CreateCreditCardAsync(PluggyAccount input)
        {
            var creditCardAlreadysaved = (await _creditCardAppService.GetAllListAsync()).Any(x => x.PluggyItemId == input.ItemId && x.PluggyCreditCardId == input.Id);
            if (!creditCardAlreadysaved)
            {
                var (bankId, bankName) = await BankFromPluggy(input.ItemId);
                var accountAlreadysaved = (await _accountAppService.GetAllListAsync()).Any(x => x.PluggyItemId == input.ItemId && x.PluggyAccountId == input.Id);

                if (!accountAlreadysaved)
                {
                    AccountFinancialDto accountFinancialDto = new AccountFinancialDto()
                    {
                        BankId = bankId,
                        AccountNickname = bankName,
                        AgencyNumber = null,
                        AccountNumber = null,
                        BalanceAvailable = (input.Balance.Value > 0) ? input.Balance.Value * -1 : input.Balance.Value,
                        AccountType = AccountType.Crédito,
                        MainAccount = false,
                        IsActive = true,
                        Origin = AccountOrigin.Pluggy,
                        PluggyItemId = input.ItemId,
                        PluggyAccountId = input.Id,
                    };

                    var accountId = await _accountAppService.CreateAndGetIdAsync(accountFinancialDto);

                    CreateCreditCardDto creditCardDto = new CreateCreditCardDto
                    {
                        CardName = input.Name,
                        CardNumber = $"**** **** **** {input.Number}",
                        AccountId = accountId,
                        FlagCardId = await FlagFromPluggy(input.CreditData.Brand),
                        Limit = input.CreditData.CreditLimit.Value,
                        DueDay = input.CreditData.BalanceDueDate.Day,
                        IsActive = true,
                        Origin = AccountOrigin.Pluggy,
                        PluggyItemId = input.ItemId,
                        PluggyCreditCardId = input.Id,
                    };

                    await _creditCardAppService.CreateAsync(creditCardDto);
                }
            }
        }

        #region PRIVATE METHODS

        private async Task<(long Id, string BankName)> BankFromPluggy(string itemId)
        {
            var bankPluggy = await _pluggyManager.PluggyGetItemIdAsync(itemId);
            var findBank = (await _accountAppService.GetAllListBanksAsync())
                .Where(x => bankPluggy.Connector.Name.ToLower().Contains(x.TradeName.ToLower())).FirstOrDefault();

            return (findBank.Id, findBank.Name);
        }

        private async Task<long> FlagFromPluggy(string flag)
        {
            var findFlag = (await _creditCardAppService.GetAllListFlagsAsync())
                .Where(x => x.Name.ToLower().Trim() == flag.ToLower().Trim()).FirstOrDefault();

            return findFlag.Id;
        }

        private (string AgencyNumber, string AccountNumber) ExtractAgencyAndAccount(string accountAgencyNumber)
        {
            var splitString = accountAgencyNumber.Split('/');

            var agencyNumber = splitString[1].Trim();
            var accountNumber = splitString[2].Trim();

            if (splitString.Length > 3 )
            {
                accountNumber = accountNumber + "/" + splitString[3].Trim();
            }

            return (agencyNumber, accountNumber);
        }
        #endregion
    }
}
