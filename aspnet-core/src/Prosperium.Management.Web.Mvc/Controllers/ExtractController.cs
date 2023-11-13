using Microsoft.AspNetCore.Mvc;
using Prosperium.Management.Controllers;
using Prosperium.Management.OpenAPI.V1.Transactions;
using System.Linq;
using System.Threading.Tasks;

namespace Prosperium.Management.Web.Controllers
{
    [Route("App/Extract")]
    public class ExtractController : ManagementControllerBase
    {
        private readonly ITransactionAppService _transactionAppService;

        public ExtractController(ITransactionAppService transactionAppService)
        {
            _transactionAppService = transactionAppService;
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
    }
}
