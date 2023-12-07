using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using static Prosperium.Management.OpenAPI.V1.Customers.CustomerConsts;

namespace Prosperium.Management.OpenAPI.V1.Customers
{
    [Table("Pxp_CustomerEmails")]
    public class CustomerEmails : Entity, IMustHaveTenant, ISoftDelete
    {
        public int TenantId { get; set; }
        public Customer Customer { get; set; }
        public long CustomerId { get; set; }
        public EmailType Type { get; set; }
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
    }
}
