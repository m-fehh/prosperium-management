using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Prosperium.Management.Authorization;
using Prosperium.Management.Controllers;
using Prosperium.Management.MultiTenancy;
using Prosperium.Management.Users;

namespace Prosperium.Management.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Tenants)]
    public class TenantsController : ManagementControllerBase
    {
        private readonly ITenantAppService _tenantAppService;
        private readonly IUserAppService _userAppService;

        public TenantsController(ITenantAppService tenantAppService, IUserAppService userAppService)
        {
            _tenantAppService = tenantAppService;
            _userAppService = userAppService;
        }

        public ActionResult Index() => View();

        public async Task<ActionResult> EditModal(int tenantId)
        {
            var tenantDto = await _tenantAppService.GetAsync(new EntityDto(tenantId));
            return PartialView("_EditModal", tenantDto);
        }

        public async Task<ActionResult> GetUserByTenantId(int tenantId)
        {
            var userDto = await _userAppService.GetUserByTenantId(tenantId);
            return PartialView("_GetUserByTenantModal", userDto);
        }
    }
}
