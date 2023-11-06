using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Prosperium.Management.Controllers;

namespace Prosperium.Management.Web.Controllers
{
    [AbpMvcAuthorize]
    public class AboutController : ManagementControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}
