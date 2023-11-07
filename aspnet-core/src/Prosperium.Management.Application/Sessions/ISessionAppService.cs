using System.Threading.Tasks;
using Abp.Application.Services;
using Prosperium.Management.Sessions.Dto;

namespace Prosperium.Management.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
