﻿using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
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
using Prosperium.Management.Plans;
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
        private readonly PlansManager _plansManager;

        public AccountAppService(IRepository<AccountFinancial, long> accountFinancialRepository, IRepository<Bank, long> banksRepository, IRepository<Category, long> categoryRepository, ITransactionAppService transactionAppService, IUnitOfWorkManager unitOfWorkManager, PluggyManager pluggyManager, ICreditCardAppService creditCardAppService, PlansManager plansManager)
        {
            _accountFinancialRepository = accountFinancialRepository;
            _banksRepository = banksRepository;
            _categoryRepository = categoryRepository;
            _transactionAppService = transactionAppService;
            _unitOfWorkManager = unitOfWorkManager;
            _pluggyManager = pluggyManager;
            _creditCardAppService = creditCardAppService;
            _plansManager = plansManager;
        }


        #region Banks 

        [HttpGet]
        [Route("list-banks")]
        public async Task<List<BankDto>> GetAllListBanksAsync()
        {
            List<Bank> allBanks = await _banksRepository.GetAllListAsync();
            return ObjectMapper.Map<List<BankDto>>(allBanks);
        }

        #endregion

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

        public async Task<bool> ValidateAccounts()
        {
            return await _plansManager.ValidatesCreatedAccounts(AbpSession.TenantId.Value);

        }

        [HttpPost]
        public async Task CreateAsync(AccountFinancialDto input)
        {
            var validationPlan = await ValidateAccounts();
            if (!validationPlan)
            {
                throw new UserFriendlyException("Limite de contas atingido. Considere aumentar seu plano para criar mais contas.");
            }

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
    }
}
