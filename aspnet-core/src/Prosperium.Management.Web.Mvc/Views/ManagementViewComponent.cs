using Abp.AspNetCore.Mvc.ViewComponents;

namespace Prosperium.Management.Web.Views
{
    public abstract class ManagementViewComponent : AbpViewComponent
    {
        protected ManagementViewComponent()
        {
            LocalizationSourceName = ManagementConsts.LocalizationSourceName;
        }
    }
}
