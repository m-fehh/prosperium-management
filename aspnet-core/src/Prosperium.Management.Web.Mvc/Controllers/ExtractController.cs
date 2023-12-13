using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Prosperium.Management.Controllers;
using Prosperium.Management.OpenAPI.V1.Accounts;
using Prosperium.Management.OpenAPI.V1.Tags;
using Prosperium.Management.OpenAPI.V1.Categories;
using Prosperium.Management.OpenAPI.V1.CreditCards;
using Prosperium.Management.OpenAPI.V1.Transactions;
using Prosperium.Management.Web.Models.Extract;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Prosperium.Management.OpenAPI.V1.Transactions.TransactionConsts;
using Abp.Extensions;

namespace Prosperium.Management.Web.Controllers
{
    [Route("App/Extract")]
    public class ExtractController : ManagementControllerBase
    {
        private readonly ITransactionAppService _transactionAppService;
        private readonly ICategoryAppService _categoryAppService;
        private readonly IAccountAppService _accountAppService;
        private readonly ICreditCardAppService _creditCardAppService;
        private readonly ITagAppService _tagAppService;

        public ExtractController(ITransactionAppService transactionAppService, ICategoryAppService categoryAppService, IAccountAppService accountAppService, ICreditCardAppService creditCardAppService, ITagAppService tagAppService)
        {
            _transactionAppService = transactionAppService;
            _categoryAppService = categoryAppService;
            _accountAppService = accountAppService;
            _creditCardAppService = creditCardAppService;
            _tagAppService = tagAppService;
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
    }
}
