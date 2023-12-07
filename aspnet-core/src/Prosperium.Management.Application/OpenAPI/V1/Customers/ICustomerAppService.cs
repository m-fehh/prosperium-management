using Abp.Application.Services;
using System.Threading.Tasks;

namespace Prosperium.Management.OpenAPI.V1.Customers
{
    public interface ICustomerAppService : IApplicationService
    {
        Task PluggyCreateCustomer(string itemId);
    }
}
