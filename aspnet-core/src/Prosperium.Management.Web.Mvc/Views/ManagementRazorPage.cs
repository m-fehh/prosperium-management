using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace Prosperium.Management.Web.Views
{
    public abstract class ManagementRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected ManagementRazorPage()
        {
            LocalizationSourceName = ManagementConsts.LocalizationSourceName;
        }
    }
}
