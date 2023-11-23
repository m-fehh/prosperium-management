using System.Threading.Tasks;
using Abp.Application.Services;
using Prosperium.Management.Authorization.Accounts.Dto;
using Prosperium.Management.Authorization.Impersonate;

namespace Prosperium.Management.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);

        Task<ImpersonateOutput> Impersonate(ImpersonateInput input);
        Task<ImpersonateOutput> BackToImpersonator();
    }
}
