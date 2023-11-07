using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prosperium.Management.OpenAPI.V1.Categories
{
    [Table("P_Categories")]
    public class Category : Entity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string Name { get; set; }
        public long? SubcategoryId { get; set; }
    }
}
