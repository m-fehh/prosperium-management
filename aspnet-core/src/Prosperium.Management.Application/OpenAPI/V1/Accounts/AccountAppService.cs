using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Prosperium.Management.Banks;
using Prosperium.Management.OpenAPI.V1.Accounts.Dto;
using Prosperium.Management.OpenAPI.V1.Transactions;
using Prosperium.Management.OpenAPI.V1.Transactions.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prosperium.Management.OpenAPI.V1.Accounts
{
    [Route("v1/accounts")]
    public class AccountAppService : ManagementAppServiceBase, IAccountAppService
    {
        private readonly IRepository<AccountFinancial, long> _accountFinancialRepository;
        private readonly IRepository<Bank, long> _banksRepository;

        private readonly ITransactionAppService _transactionAppService;

        public AccountAppService(IRepository<AccountFinancial, long> accountFinancialRepository, IRepository<Bank, long> banksRepository, ITransactionAppService transactionAppService)
        {
            _accountFinancialRepository = accountFinancialRepository;
            _banksRepository = banksRepository;
            _transactionAppService = transactionAppService;
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
            List<AccountFinancial> allAccounts = await _accountFinancialRepository.GetAllListAsync();
            return ObjectMapper.Map<List<AccountFinancialDto>>(allAccounts);
        }

        [HttpPost]
        public async Task CreateAsync(AccountFinancialDto input)
        {
            AccountFinancial account = ObjectMapper.Map<AccountFinancial>(input);
            await _accountFinancialRepository.InsertAndGetIdAsync(account);

            // Quando adicionado uma conta, deve constar o valor no extrato

            var extractCreated = new TransactionDto
            {
                Date = DateTime.Now,
                Description = $"Saldo da conta: {input.AccountNickname}",
                ExpenseValue = input.BalanceAvailable,
                AccountId = account.Id
            };

            await _transactionAppService.CreateAsync(extractCreated);
        }
    }
}
