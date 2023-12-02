using Microsoft.AspNetCore.Mvc;
using Prosperium.Management.Controllers;
using Prosperium.Management.OpenAPI.V1.Accounts;
using Prosperium.Management.OpenAPI.V1.Accounts.Dto;
using Prosperium.Management.OpenAPI.V1.Categories;
using Prosperium.Management.OpenAPI.V1.Categories.Dto;
using Prosperium.Management.Web.Models.OriginDestinations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prosperium.Management.Web.Controllers
{
    [Route("App/OriginDestinations")]
    public class OriginDestinationsController : ManagementControllerBase
    {
        private readonly ICategoryAppService _categoryAppService;
        private readonly IAccountAppService _accountAppService;

        public OriginDestinationsController(ICategoryAppService categoryAppService, IAccountAppService accountAppService)
        {
            _categoryAppService = categoryAppService;
            _accountAppService = accountAppService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("GetDestinations")]
        public async Task<IActionResult> GetDestinations(int pluggyId, string discriminator)
        {
            var model = new OriginDestinationModalViewModel
            {
                PluggyId = pluggyId
            };

            switch (discriminator.ToLower().Trim())
            {
                case "bancos":
                    model.Banks = await GetDestinationsForBanks();
                    break;
                case "categoria":
                    model.Categories = await GetDestinationsForCategories();
                    break;
            }

            return PartialView("_EditModal", model);
        }

        private async Task<List<CategoryDto>> GetDestinationsForCategories()
        {
            return await _categoryAppService.GetAllListAsync();

        }

        private async Task<List<BankDto>> GetDestinationsForBanks()
        {
            return await _accountAppService.GetAllListBanksAsync();

        }
    }
}
