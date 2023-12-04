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

        [HttpGet]
        [Route("GetValuesTotals")]
        public async Task<IActionResult> GetValuesTotals(string filter, string monthYear, string filteredAccounts, string filteredCards, string filteredTags, string filteredCategories, string filteredTypes)
        {
            var allTransactions = await _transactionAppService.GetAllListAsync();

            // Aplicar filtros semelhantes aos do método GetAllAsync

            if (!string.IsNullOrEmpty(filter))
            {
                allTransactions = allTransactions
                    .Where(x => x.Description.ToLower().Trim().Contains(filter.ToLower().Trim()))
                    .ToList();
            }


            if (!string.IsNullOrEmpty(filteredAccounts))
            {
                monthYear = null;

                var accountIds = filteredAccounts.Split(',').Select(id => long.Parse(id)).ToList();
                allTransactions = allTransactions.Where(x => x.AccountId.HasValue && accountIds.Contains(x.AccountId.Value)).ToList();
            }

            if (!string.IsNullOrEmpty(filteredCards))
            {
                monthYear = null;

                var cardIds = filteredCards.Split(',').Select(id => long.Parse(id)).ToList();
                allTransactions = allTransactions.Where(x => x.CreditCardId.HasValue && cardIds.Contains(x.CreditCardId.Value)).ToList();
            }

            if (!string.IsNullOrEmpty(filteredCategories))
            {
                monthYear = null;

                var categoryIds = filteredCategories.Split(',').Select(id => long.Parse(id)).ToList();
                allTransactions = allTransactions.Where(x => categoryIds.Contains(x.CategoryId)).ToList();
            }

            if (!string.IsNullOrEmpty(filteredTags))
            {
                var tagIds = filteredTags.Split(',').Select(id => long.Parse(id)).ToList();
                allTransactions = allTransactions.Where(x => x.Tags.Any(tag => tagIds.Contains(tag.Id))).ToList();
            }

            if (!string.IsNullOrEmpty(filteredTypes))
            {
                monthYear = null;

                var transactionTypes = filteredTypes.Split(',').Select(type => Enum.Parse(typeof(TransactionType), type)).Cast<TransactionType>().ToList();
                allTransactions = allTransactions.Where(x => transactionTypes.Contains(x.TransactionType)).ToList();
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

            decimal gastos = Math.Abs(allTransactions.Where(x => x.TransactionType == TransactionConsts.TransactionType.Gastos).Sum(x => x.ExpenseValue));
            decimal ganhos = Math.Abs(allTransactions.Where(x => x.TransactionType == TransactionConsts.TransactionType.Ganhos).Sum(x => x.ExpenseValue));

            var resultado = new { gastos, ganhos };

            return Json(resultado);
        }


        [HttpGet("GetAllFilters")]
        public async Task<ActionResult> GetAllFilters()
        {
            var allTransactions = await _transactionAppService.GetAllListAsync();

            var allAccounts = await _accountAppService.GetAllListAsync();
            var allCards = await _creditCardAppService.GetAllListAsync();
            var allTags = await _tagAppService.GetTagsListAsync();
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
                Cards = allCards,
                Tags = allTags,
                Categories = allCategories,
                TransactionType = allTransactionTypes,
                TransactionQuantity = allTransactions.Count()
            };

            return PartialView("_FilterExtractModal", model);
        }
    }
}
