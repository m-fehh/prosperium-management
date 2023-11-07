using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Prosperium.Management.OpenAPI.V1.Subcategories.Dto;

namespace Prosperium.Management.OpenAPI.V1.Categories.Dto
{
    public class CategoryDto : EntityDto<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string Name { get; set; }
        public long? SubcategoryId { get; set; }
    }
}
