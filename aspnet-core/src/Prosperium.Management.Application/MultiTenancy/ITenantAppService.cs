using Abp.Application.Services;
using Prosperium.Management.MultiTenancy.Dto;

namespace Prosperium.Management.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

