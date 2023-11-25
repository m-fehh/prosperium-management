using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Prosperium.Management.Controllers;
using System.Threading.Tasks;
using Prosperium.Management.OpenAPI.V1.Transactions;
using System.Linq;
using static Prosperium.Management.OpenAPI.V1.Transactions.TransactionConsts;

namespace Prosperium.Management.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : ManagementControllerBase
    {
        private readonly ITransactionAppService _transactionAppService;

        public HomeController(ITransactionAppService transactionAppService)
        {
            _transactionAppService = transactionAppService;
        }

        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("GetValuesTotals")]
        public async Task<IActionResult> GetValuesTotals(string monthYear)
        {
            var allTransactions = await _transactionAppService.GetAllListAsync();

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

        [HttpGet]
        [Route("GetTransactionsByCategory")]
        public async Task<IActionResult> GetTransactionsByCategory(string monthYear, int transactionType)
        {
            var allTransactions = await _transactionAppService.GetAllListAsync();

            if (!string.IsNullOrEmpty(monthYear))
            {
                var parts = monthYear.Split('/');
                var month = int.Parse(parts[0]);
                var year = int.Parse(parts[1]);

                allTransactions = allTransactions
                    .Where(x => x.Date.Month == month && x.Date.Year == year)
                    .ToList();
            }

            if(transactionType > 0)
            {
                var transactionTypeEnum = (TransactionType) transactionType;

                allTransactions = allTransactions.Where(x => x.TransactionType == transactionTypeEnum).ToList();
            }

            var transactionsByCategory = allTransactions
                .GroupBy(t => t.Categories.Name)
                .Select(group => new
                    {
                        Categoria = group.Key,
                        ImageCategory = group.Select(x => x.Categories.IconPath),
                        Contagem = group.Count(),
                        ValorTotal = group.Sum(t => t.ExpenseValue)
                    })
                .OrderByDescending(item => item.Contagem)
                .Take(5);

            return Json(transactionsByCategory);
        }

        [HttpGet]
        [Route("LatestTransactions")]
        public async Task<IActionResult> LatestTransactions(string monthYear)
        {
            var allTransactions = await _transactionAppService.GetAllListAsync();

            if (!string.IsNullOrEmpty(monthYear))
            {
                var parts = monthYear.Split('/');
                var month = int.Parse(parts[0]);
                var year = int.Parse(parts[1]);

                allTransactions = allTransactions
                    .Where(x => x.Date.Month == month && x.Date.Year == year)
                    .ToList();
            }

            var latestTransactions = allTransactions
                .OrderByDescending(t => t.Date)
                .Take(4)
                .Select(t => new
                {
                    TransactionDate = t.Date,
                    Category = t.Categories.Name,
                    ImageCategory = t.Categories.IconPath,
                    ExpenseValue = t.ExpenseValue,
                    TransactionType = t.TransactionType
                });

            return Json(latestTransactions);
        }
    }
}
