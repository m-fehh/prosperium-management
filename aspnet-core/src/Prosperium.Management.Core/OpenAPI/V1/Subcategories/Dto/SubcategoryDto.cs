using Abp.Application.Services.Dto;
using Abp.Domain.Entities;

namespace Prosperium.Management.OpenAPI.V1.Subcategories.Dto
{
    public class SubcategoryDto : EntityDto<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string Name { get; set; }
    }
}
