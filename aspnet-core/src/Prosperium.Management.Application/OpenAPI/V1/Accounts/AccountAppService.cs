using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Prosperium.Management.Banks;
using Prosperium.Management.OpenAPI.V1.Accounts.Dto;
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

        public AccountAppService(IRepository<AccountFinancial, long> accountFinancialRepository, IRepository<Bank, long> banksRepository)
        {
            _accountFinancialRepository = accountFinancialRepository;
            _banksRepository = banksRepository;
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
            await _accountFinancialRepository.InsertAsync(account);
        }
    }
}
