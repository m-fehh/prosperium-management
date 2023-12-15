using Microsoft.AspNetCore.Mvc;
using Prosperium.Management.Controllers;

namespace Prosperium.Management.Web.Controllers
{
    [Route("App/Opportunities")]
    public class OpportunitiesController : ManagementControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
