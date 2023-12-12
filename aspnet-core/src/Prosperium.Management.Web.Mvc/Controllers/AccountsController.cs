﻿using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Prosperium.Management.Controllers;
using Prosperium.Management.ExternalServices.Pluggy;
using Prosperium.Management.OpenAPI.V1.Accounts;
using Prosperium.Management.OpenAPI.V1.Accounts.Dto;
using Prosperium.Management.OpenAPI.V1.Transactions;
using Prosperium.Management.Web.Models.Accounts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Prosperium.Management.Web.Controllers
{
    [Route("App/Accounts")]
    public class AccountsController : ManagementControllerBase
    {
        private readonly IAccountAppService _accountsAppService;
        private readonly IPluggyAppService _pluggyAppService;
        private readonly ITransactionAppService _transactionAppService;
        private readonly PluggyManager _pluggyManager;

        public AccountsController(IAccountAppService accountsAppService, IPluggyAppService pluggyAppService, ITransactionAppService transactionAppService, PluggyManager pluggyManager)
        {
            _accountsAppService = accountsAppService;
            _pluggyAppService = pluggyAppService;
            _transactionAppService = transactionAppService;
            _pluggyManager = pluggyManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("Create", Name = "CreateAccount")]
        public async Task<IActionResult> CreateAccount()
        {
            var viewModel = new AccountFinancialDto();

            return View(viewModel);
        }

        [HttpGet]
        [Route("GetAccounts")]
        public async Task<IActionResult> GetAccounts(bool isActive)
        {
            // Busca as contas da Pluggy

            var allAccounts = await _accountsAppService.GetAllListAsync();
            allAccounts = allAccounts.Where(x => x.IsActive == isActive).ToList();

            return Json(allAccounts);
        }

        [HttpGet]
        [Route("GetBanks")]
        public async Task<IActionResult> GetBanks()
        {
            var allBanks = await _accountsAppService.GetAllListBanksAsync();

            var model = new CreateAccountViewModel
            {
                Banks = allBanks
            };

            return PartialView("_SelectInstitutionModal", model);
        }

        [HttpGet]
        [Route("GetValuesTotals")]
        public async Task<IActionResult> GetValuesTotals(long accountId)
        {
            var allTransactions = await _transactionAppService.GetAllTransactionPerAccount(accountId);
            var account = await _accountsAppService.GetAccountById(accountId);

            decimal gastos = allTransactions.Where(x => x.TransactionType == TransactionConsts.TransactionType.Gastos).Sum(x => x.ExpenseValue);
            decimal ganhos = allTransactions.Where(x => x.TransactionType == TransactionConsts.TransactionType.Ganhos).Sum(x => x.ExpenseValue);

            decimal total = Math.Abs(account.BalanceAvailable) + Math.Abs(ganhos) - Math.Abs(gastos);
            var resultado = new { gastos, ganhos, total };

            return Json(resultado);
        }

        [HttpGet]
        [Route("PluggyGetAccessToken")]
        public async Task<IActionResult> PluggyGetAccessToken()
        {
            try
            {
                var validationPlan = await _accountsAppService.ValidateAccounts();
                if (!validationPlan)
                {
                    throw new UserFriendlyException("Limite de contas atingido. Considere aumentar seu plano para criar mais contas.");
                }

                var accessToken = await _pluggyManager.PluggyCreateConnectTokenAsync();
                var result = new { accessToken };

                return Json(result);
            }
            catch (UserFriendlyException ex)
            {
                // Log ou retornar a mensagem de exceção
                return Json(new { error = ex.Message });
            }
        }


        [HttpPost]
        [Route("InsertAccountPluggy")]
        public async Task InsertAccountPluggy([FromBody] string itemId)
        {
            await _pluggyAppService.PluggyCreateAsync(itemId);

        }
    }
}
