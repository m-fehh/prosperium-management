using System.Threading.Tasks;
using Abp.Application.Services;
using Prosperium.Management.Authorization.Accounts.Dto;

namespace Prosperium.Management.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
