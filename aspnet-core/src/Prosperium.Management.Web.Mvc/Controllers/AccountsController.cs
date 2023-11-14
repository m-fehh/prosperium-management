using Microsoft.AspNetCore.Mvc;
using Prosperium.Management.Controllers;
using Prosperium.Management.OpenAPI.V1.Accounts;
using Prosperium.Management.OpenAPI.V1.Accounts.Dto;
using Prosperium.Management.Web.Models.Accounts;
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
            var banks = await _accountsAppService.GetAllListBanksAsync();
            var viewModel = new CreateAccountViewModel
            {
                Banks = banks,
                Account = new AccountFinancialDto(),
            };

            return View(viewModel);
        }

        [HttpGet]
        [Route("GetAccounts")]
        public async Task<IActionResult> GetAccounts()
        {
            var allAccounts = await _accountsAppService.GetAllListAsync();
            return Json(allAccounts);
        }
    }
}
