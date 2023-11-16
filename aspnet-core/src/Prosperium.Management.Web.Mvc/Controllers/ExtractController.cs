using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Prosperium.Management.Controllers;
using Prosperium.Management.OpenAPI.V1.Accounts;
using Prosperium.Management.OpenAPI.V1.Categories;
using Prosperium.Management.OpenAPI.V1.Transactions;
using Prosperium.Management.Web.Models.Extract;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Prosperium.Management.OpenAPI.V1.Transactions.TransactionConsts;

namespace Prosperium.Management.Web.Controllers
{
    [Route("App/Extract")]
    public class ExtractController : ManagementControllerBase
    {
        private readonly ITransactionAppService _transactionAppService;

        private readonly ICategoryAppService _categoryAppService;
        private readonly IAccountAppService _accountAppService;

        public ExtractController(ITransactionAppService transactionAppService, ICategoryAppService categoryAppService, IAccountAppService accountAppService)
        {
            _transactionAppService = transactionAppService;
            _categoryAppService = categoryAppService;
            _accountAppService = accountAppService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("GetValuesTotals")]
        public async Task<IActionResult> GetValuesTotals(string filter, string monthYear)
        {
            var allTransactions = await _transactionAppService.GetAllListAsync();

            if (!string.IsNullOrEmpty(filter))
            {
                allTransactions = allTransactions
                    .Where(x => x.Description.ToLower().Trim().Contains(filter.ToLower().Trim()))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(monthYear))
            {
                var parts = monthYear.Split('/');
                var month = int.Parse(parts[0]);
                var year = int.Parse(parts[1]);

                allTransactions = allTransactions
                    .Where(x => x.Date.Month == month && x.Date.Year == year)
                    .ToList();
            }

            decimal gastos = allTransactions.Where(x => x.TransactionType == TransactionConsts.TransactionType.Gastos).Sum(x => x.ExpenseValue);
            decimal ganhos = allTransactions.Where(x => x.TransactionType == TransactionConsts.TransactionType.Ganhos).Sum(x => x.ExpenseValue);

            var resultado = new { gastos, ganhos };

            return Json(resultado);
        }

        [HttpGet("GetAllFilters")]
        public async Task<ActionResult> GetAllFilters()
        {
            var allAccounts = await _accountAppService.GetAllListAsync();
            var allCategories = await _categoryAppService.GetAllListPerTenantAsync();
            var allTransactionTypes = Enum.GetValues(typeof(TransactionType))
                               .Cast<TransactionType>()
                               .Select(tt => new SelectListItem
                               {
                                   Value = ((int)tt).ToString(),
                                   Text = tt.ToString()
                               })
                               .ToList();

            var model = new AllFiltersModalViewModel
            {
                Accounts = allAccounts,
                Categories = allCategories,
                TransactionType = allTransactionTypes
            };

            return PartialView("_FilterExtractModal", model);
        }
    }
}
