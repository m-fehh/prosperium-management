using Microsoft.AspNetCore.Mvc;
using Prosperium.Management.Controllers;
using Prosperium.Management.OpenAPI.V1.Accounts;
using Prosperium.Management.OpenAPI.V1.Accounts.Dto;
using Prosperium.Management.Web.Models.Accounts;
using System.Linq;
using System.Threading.Tasks;

namespace Prosperium.Management.Web.Controllers
{
    [Route("App/Accounts")]
    public class AccountsController : ManagementControllerBase
    {
        private readonly IAccountAppService _accountsAppService;

        public AccountsController(IAccountAppService accountsAppService)
        {
            _accountsAppService = accountsAppService;
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
    }
}
