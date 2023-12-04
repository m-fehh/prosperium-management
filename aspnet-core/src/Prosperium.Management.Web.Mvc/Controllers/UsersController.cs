using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Prosperium.Management.Authorization;
using Prosperium.Management.Controllers;
using Prosperium.Management.Users;
using Prosperium.Management.Web.Models.Users;
using Abp.Domain.Repositories;
using Prosperium.Management.MultiTenancy;

namespace Prosperium.Management.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Users)]
    public class UsersController : ManagementControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly IRepository<Tenant> _tenantRepository;

        public UsersController(IUserAppService userAppService, IRepository<Tenant> tenantRepository)
        {
            _userAppService = userAppService;
            _tenantRepository = tenantRepository;
        }

        public async Task<ActionResult> Index()
        {
            var roles = (await _userAppService.GetRoles()).Items;
            var tenants = await _tenantRepository.GetAllListAsync();
            var model = new UserListViewModel
            {
                Tenants = tenants,
                Roles = roles
            };
            return View(model);
        }

        public async Task<ActionResult> EditModal(long userId)
        {
            var user = await _userAppService.GetAsync(new EntityDto<long>(userId));
            var roles = (await _userAppService.GetRoles()).Items;
            var model = new EditUserModalViewModel
            {
                User = user,
                Roles = roles
            };
            return PartialView("_EditModal", model);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
    }
}
