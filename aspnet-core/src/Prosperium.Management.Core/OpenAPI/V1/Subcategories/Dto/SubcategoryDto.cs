using Abp.Application.Services.Dto;
using Abp.Domain.Entities;

namespace Prosperium.Management.OpenAPI.V1.Subcategories.Dto
{
    public class SubcategoryDto : EntityDto<long>
    {
        public string Name { get; set; }
        public long CategoryId { get; set; }
    }
}
