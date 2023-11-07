using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prosperium.Management.OpenAPI.V1.Tags
{
    [Table("P_Tags")]
    public class Tag : Entity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string Name { get; set; }
    }
}
