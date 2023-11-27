using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prosperium.Management.Banks;
using Prosperium.Management.ExternalServices.Pluggy;
using Prosperium.Management.OpenAPI.V1.Accounts.Dto;
using Prosperium.Management.OpenAPI.V1.Categories;
using Prosperium.Management.OpenAPI.V1.Transactions;
using Prosperium.Management.OpenAPI.V1.Transactions.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

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

        public AccountAppService(IRepository<AccountFinancial, long> accountFinancialRepository, IRepository<Bank, long> banksRepository, IRepository<Category, long> categoryRepository, ITransactionAppService transactionAppService, IUnitOfWorkManager unitOfWorkManager, PluggyManager pluggyManager)
        {
            _accountFinancialRepository = accountFinancialRepository;
            _banksRepository = banksRepository;
            _categoryRepository = categoryRepository;
            _transactionAppService = transactionAppService;
            _unitOfWorkManager = unitOfWorkManager;
            _pluggyManager = pluggyManager;
        }


        [HttpGet]
        [Route("list-banks")]
        public async Task<List<BankDto>> GetAllListBanksAsync()
        {
            List<Bank> allBanks = await _banksRepository.GetAllListAsync();
            return ObjectMapper.Map<List<BankDto>>(allBanks);
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

        #region Pluggy API 

        [HttpPost]
        [Route("PluggyGetAcessToken")]
        public async Task PluggyGetAcessToken()
        {
            string accessToken = await _pluggyManager.PluggyCreateConnectToken();
        }
        #endregion
    }
}
