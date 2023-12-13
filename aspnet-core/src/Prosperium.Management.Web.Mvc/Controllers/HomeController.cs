using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Prosperium.Management.Controllers;
using System.Threading.Tasks;
using System.Linq;
using Prosperium.Management.OpenAPI.V1.CreditCards;
using Prosperium.Management.OpenAPI.V1.Accounts;
using Prosperium.Management.Web.Models.Extract;
using Prosperium.Management.OpenAPI.V1.Categories;

namespace Prosperium.Management.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : ManagementControllerBase
    {
        private readonly ICreditCardAppService _creditCardAppService;
        private readonly IAccountAppService _accountAppService;
        private readonly ICategoryAppService _categoryAppService;

        public HomeController(ICreditCardAppService creditCardAppService, IAccountAppService accountAppService, ICategoryAppService categoryAppService)
        {
            _creditCardAppService = creditCardAppService;
            _accountAppService = accountAppService;
            _categoryAppService = categoryAppService;
        }

        [Route("")]
        public ActionResult Index()
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
