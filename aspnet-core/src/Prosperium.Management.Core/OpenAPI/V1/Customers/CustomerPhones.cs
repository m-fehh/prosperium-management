using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using static Prosperium.Management.OpenAPI.V1.Customers.CustomerConsts;

namespace Prosperium.Management.OpenAPI.V1.Customers
{
    [Table("Pxp_CustomerPhones")]
    public class CustomerPhones : Entity, IMustHaveTenant, ISoftDelete
    {
        public int TenantId { get; set; }
        public Customer Customer { get; set; }
        public long CustomerId { get; set; }
        public PhoneNumberType Type { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsDeleted { get; set; }
    }
}
