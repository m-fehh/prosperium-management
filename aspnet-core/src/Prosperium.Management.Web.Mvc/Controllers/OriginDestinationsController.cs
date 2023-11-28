using Microsoft.AspNetCore.Mvc;
using Prosperium.Management.Controllers;
using Prosperium.Management.OpenAPI.V1.Categories;
using Prosperium.Management.Web.Models.OriginDestinations;
using System.Threading.Tasks;

namespace Prosperium.Management.Web.Controllers
{
    [Route("App/OriginDestinations")]
    public class OriginDestinationsController : ManagementControllerBase
    {
        private readonly ICategoryAppService _categoryAppService;

        public OriginDestinationsController(ICategoryAppService categoryAppService)
        {
            _categoryAppService = categoryAppService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("GetDestinations")]
        public async Task<IActionResult> GetDestinations(int pluggyId)
        {
            var allCategoriesProsperium = await _categoryAppService.GetAllListAsync();

            var model = new OriginDestinationModalViewModel
            {
                Categories = allCategoriesProsperium,
                PluggyId = pluggyId
            };

            return PartialView("_EditModal", model);
        }
    }
}
