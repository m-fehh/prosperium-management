using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Prosperium.Management.Authorization;
using Prosperium.Management.Controllers;
using Prosperium.Management.MultiTenancy;
using Prosperium.Management.Users;
using Abp.Domain.Repositories;
using Prosperium.Management.Web.Models.Tenants;
using Prosperium.Management.Plans;

namespace Prosperium.Management.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Tenants)]
    public class TenantsController : ManagementControllerBase
    {
        private readonly IRepository<Plan> _planRepository;
        private readonly ITenantAppService _tenantAppService;
        private readonly IUserAppService _userAppService;
        private readonly PlansManager _planManager;

        public TenantsController(IRepository<Plan> planRepository, ITenantAppService tenantAppService, IUserAppService userAppService, PlansManager planManager)
        {
            _planRepository = planRepository;
            _tenantAppService = tenantAppService;
            _userAppService = userAppService;
            _planManager = planManager;
        }

        public ActionResult Index() => View();

        public async Task<ActionResult> EditModal(int tenantId)
        {
            var tenantDto = await _tenantAppService.GetAsync(new EntityDto(tenantId));
            return PartialView("_EditModal", tenantDto);
        }

        public async Task<ActionResult> EditPlan(int tenantId)
        {
            var plans = await _planRepository.GetAllListAsync();

            var tenantDto = await _tenantAppService.GetAsync(new EntityDto(tenantId));

            var model = new AlterPlans
            {
                Plans = plans,
                Tenant = tenantDto
            };

            return PartialView("_EditPlan", model);
        }

        public async Task UpdatePlanByTenant(int tenantId, int selectedPlanId, string selectedPlanName, string formattedDate)
        {
            await _planManager.ChangeTenantPlan(tenantId, selectedPlanId, selectedPlanName, formattedDate);
        }

        public async Task<ActionResult> GetUserByTenantId(int tenantId)
        {
            var userDto = await _userAppService.GetUserByTenantId(tenantId);
            return PartialView("_GetUserByTenantModal", userDto);
        }
    }
}
