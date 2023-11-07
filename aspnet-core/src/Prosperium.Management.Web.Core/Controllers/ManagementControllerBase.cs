using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Prosperium.Management.Controllers
{
    public abstract class ManagementControllerBase: AbpController
    {
        protected ManagementControllerBase()
        {
            LocalizationSourceName = ManagementConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
