using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prosperium.Management.OpenAPI.V1.Tags
{
    [Table("Pxp_Tags")]
    public class Tag : AuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string Name { get; set; }
    }
}
