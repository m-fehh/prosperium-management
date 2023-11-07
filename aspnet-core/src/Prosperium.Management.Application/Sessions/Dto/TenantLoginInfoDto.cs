using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Prosperium.Management.MultiTenancy;

namespace Prosperium.Management.Sessions.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}
