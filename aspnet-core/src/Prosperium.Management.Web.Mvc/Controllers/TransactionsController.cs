using Microsoft.AspNetCore.Mvc;
using Prosperium.Management.Controllers;

namespace Prosperium.Management.Web.Controllers
{
    [Route("App/Transactions")]
    public class TransactionsController : ManagementControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
