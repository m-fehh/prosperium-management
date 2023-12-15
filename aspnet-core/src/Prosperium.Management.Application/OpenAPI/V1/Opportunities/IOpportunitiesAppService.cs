using Abp.Application.Services;
using System.Threading.Tasks;

namespace Prosperium.Management.OpenAPI.V1.Opportunities
{
    public interface IOpportunitiesAppService : IApplicationService
    {
        Task PluggyCreateOpportunitiesAsync(string itemId);
    }
}