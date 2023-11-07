using Abp.Domain.Entities;
using Prosperium.Management.OpenAPI.V1.Categories;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prosperium.Management.OpenAPI.V1.Subcategories
{
    [Table("P_Subcategories")]
    public class Subcategory : Entity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Category Category { get; set; }
        public string Name { get; set; }
    }
}
