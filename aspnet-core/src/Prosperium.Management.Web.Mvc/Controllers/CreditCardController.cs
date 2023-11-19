using Microsoft.AspNetCore.Mvc;
using Prosperium.Management.Controllers;
using Prosperium.Management.OpenAPI.V1.Accounts;
using Prosperium.Management.OpenAPI.V1.CreditCards;
using Prosperium.Management.OpenAPI.V1.CreditCards.Dto;
using Prosperium.Management.Web.Models.CreditCard;
using Prosperium.Management.Web.Models.Transactions;
using System.Threading.Tasks;

namespace Prosperium.Management.Web.Controllers
{
    [Route("App/CreditCard")]
    public class CreditCardController : ManagementControllerBase
    {
        private readonly ICreditCardAppService _creditCardAppService;
        private readonly IAccountAppService _accountAppService;

        public CreditCardController(ICreditCardAppService creditCardAppService, IAccountAppService accountAppService)
        {
            _creditCardAppService = creditCardAppService;
            _accountAppService = accountAppService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("Create", Name = "CreateCard")]
        public async Task<IActionResult> CreateCard()
        {
            var viewModel = new CreateCreditCardDto();
            return View(viewModel);
        }

        [HttpGet]
        [Route("GetCards")]
        public async Task<IActionResult> GetCards()
        {
            var allCard = await _creditCardAppService.GetAllListAsync();
            return Json(allCard);
        }

        [HttpGet("GetAccounts")]
        public async Task<ActionResult> GetAccounts()
        {
            var allAccounts = await _accountAppService.GetAllListAsync();
            var model = new SelectAccountModalViewModel
            {
                Accounts = allAccounts,
            };

            return PartialView("_SelectAccountModal", model);
        }

        [HttpGet]
        [Route("GetFlags")]
        public async Task<IActionResult> GetFlags()
        {
            var allFlags = await _creditCardAppService.GetAllListFlagsAsync();

            var model = new CreateCreditCardViewModel
            {
                Flags = allFlags
            };

            return PartialView("_SelectFlagsModal", model);
        }
    }
}
