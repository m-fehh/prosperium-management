﻿using Microsoft.AspNetCore.Mvc;
using Prosperium.Management.Controllers;
using Prosperium.Management.OpenAPI.V1.Accounts;
using Prosperium.Management.OpenAPI.V1.Categories;
using Prosperium.Management.OpenAPI.V1.CreditCards;
using Prosperium.Management.OpenAPI.V1.Transactions;
using Prosperium.Management.OpenAPI.V1.Transactions.Dto;
using Prosperium.Management.Web.Models.Extract;
using System.Linq;
using System.Threading.Tasks;

namespace Prosperium.Management.Web.Controllers
{
    [Route("App/Extract")]
    public class ExtractController : ManagementControllerBase
    {
        private readonly ICategoryAppService _categoryAppService;
        private readonly IAccountAppService _accountAppService;
        private readonly ITransactionAppService _transactionAppService;
        private readonly ICreditCardAppService _creditCardAppService;

        public ExtractController(ICategoryAppService categoryAppService, IAccountAppService accountAppService, ITransactionAppService transactionAppService, ICreditCardAppService creditCardAppService)
        {
            _categoryAppService = categoryAppService;
            _accountAppService = accountAppService;
            _transactionAppService = transactionAppService;
            _creditCardAppService = creditCardAppService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetAllFilters")]
        public async Task<ActionResult> GetAllFilters()
        {
            var allAccounts = (await _accountAppService.GetAllListAsync()).Where(x => x.AccountType != AccountConsts.AccountType.Crédito).ToList();
            var allCards = await _creditCardAppService.GetAllListAsync();
            var allCategories = await _categoryAppService.GetAllListPerTenantAsync();

            var model = new AllFiltersModalViewModel
            {
                Accounts = allAccounts,
                Cards = allCards,
                Categories = allCategories,
            };

            return PartialView("_FilterExtractModal", model);
        }

        [HttpGet("ViewDetail")]
        public async Task<IActionResult> ViewDetail(long transactionId)
        {
            var transaction = await _transactionAppService.GetByIdAsync(transactionId);
            var model = new DetailTransactionModalViewModel
            {
                Transaction = transaction
            };

            return PartialView("_ViewDetailTransactionModal", model);
        }

        [HttpGet("ContainsInterAccount")]
        public async Task<ActionResult<bool>> ContainsInterAccount()
        {
            var hasInterAccount = (await _accountAppService.GetAllListAsync()).Any(x => x.Bank.Name.ToUpper().Trim() == "BANCO INTER S.A.");
            return Ok(hasInterAccount);
        }
    }
}
