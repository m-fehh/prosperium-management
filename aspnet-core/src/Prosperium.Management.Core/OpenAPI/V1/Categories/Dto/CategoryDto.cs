using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Prosperium.Management.OpenAPI.V1.Subcategories;
using Prosperium.Management.OpenAPI.V1.Subcategories.Dto;
using System.Collections.Generic;
using static Prosperium.Management.OpenAPI.V1.Transactions.TransactionConsts;

namespace Prosperium.Management.OpenAPI.V1.Categories.Dto
{
    public class CategoryDto : EntityDto<long>
    {
        public string IconPath { get; set; }
        public string Name { get; set; }
        public TransactionType TransactionType { get; set; }

        public virtual ICollection<SubcategoryDto> Subcategories { get; set; }
        public bool IsVisible { get; set; }
    }
}
