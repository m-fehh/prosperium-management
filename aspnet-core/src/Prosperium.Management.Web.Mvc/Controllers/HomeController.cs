using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Prosperium.Management.Controllers;
using System.Threading.Tasks;
using Prosperium.Management.OpenAPI.V1.Transactions;
using System.Linq;
using static Prosperium.Management.OpenAPI.V1.Transactions.TransactionConsts;
using Prosperium.Management.OpenAPI.V1.CreditCards;
using Prosperium.Management.OpenAPI.V1.CreditCards.Dto;
using System;
using Microsoft.VisualBasic;

namespace Prosperium.Management.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : ManagementControllerBase
    {
        private readonly ITransactionAppService _transactionAppService;
        private readonly ICreditCardAppService _creditCardAppService;

        public HomeController(ITransactionAppService transactionAppService, ICreditCardAppService creditCardAppService)
        {
            _transactionAppService = transactionAppService;
            _creditCardAppService = creditCardAppService;
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

            decimal gastos = Math.Abs(allTransactions.Where(x => x.TransactionType == TransactionConsts.TransactionType.Gastos).Sum(x => x.ExpenseValue));
            decimal ganhos = Math.Abs(allTransactions.Where(x => x.TransactionType == TransactionConsts.TransactionType.Ganhos).Sum(x => x.ExpenseValue));
            decimal total = Math.Abs(ganhos) - Math.Abs(gastos * -1);

            string totalFormatted = total >= 0 ? $"R$ {total:N2}" : $"- R$ {Math.Abs(total):N2}";

            var resultado = new { gastos, ganhos, totalFormatted };

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

            if (transactionType > 0)
            {
                var transactionTypeEnum = (TransactionType)transactionType;

                allTransactions = allTransactions.Where(x => x.TransactionType == transactionTypeEnum).ToList();
            }

            if (allTransactions.Count > 0)
            {

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
                .Take(10);

                return Json(transactionsByCategory);
            }
            else
            {
                return Json(new { message = "Não há transações disponíveis para o mês/ano especificado." });
            }
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

            if (allTransactions.Count > 0)
            {

                var latestTransactions = allTransactions
                .OrderByDescending(t => t.Date)
                .Take(4)
                .Select(t => new
                {
                    TransactionDate = t.Date,
                    Category = t.Categories.Name,
                    ImageCategory = t.Categories.IconPath,
                    ExpenseValue = (t.ExpenseValue >= 0) ? $"R$ {t.ExpenseValue:N2}" : $"- R$ {Math.Abs(t.ExpenseValue):N2}",
                    TransactionType = t.TransactionType
                });


                return Json(latestTransactions);
            }

            else
            {
                return Json(new { message = "Não há transações disponíveis para o mês/ano especificado." });
            }
        }

        [HttpGet]
        [Route("GetCreditCardExpenses")]
        public async Task<IActionResult> GetCreditCardExpenses(string monthYear)
        {
            var allCards = await _creditCardAppService.GetAllListAsync();

            var cardTransactions = allCards.Select(card => new
            {
                CreditCard = card.CardName,
                Progress = CalculateCreditCardProgress(card, monthYear),
                Limit = card.Limit,
                Logo = card.FlagCard.IconPath,
                DueDate = card.DueDay,
                ValorGasto = card.Transactions
                    .Where(transaction => string.IsNullOrEmpty(monthYear) ||
                                         (transaction.Date.ToString("MM/yyyy") == monthYear))
                    .Sum(transaction => transaction.ExpenseValue),
            });

            return Json(cardTransactions);
        }

        private decimal CalculateCreditCardProgress(CreditCardDto creditCard, string monthYear)
        {
            decimal totalExpenses = creditCard.Transactions?
                .Where(transaction => string.IsNullOrEmpty(monthYear) || (transaction.Date.ToString("MM/yyyy") == monthYear))
                .Sum(transaction => transaction.ExpenseValue) ?? 0;

            decimal progress = (int)((Math.Abs(totalExpenses) / Math.Abs(creditCard.Limit)) * 100);

            return progress;
        }
    }
}
