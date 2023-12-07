using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using static Prosperium.Management.OpenAPI.V1.Customers.CustomerConsts;

namespace Prosperium.Management.OpenAPI.V1.Customers
{
    [Table("Pxp_CustomerAddresses")]
    public class CustomerAddresses : Entity, IMustHaveTenant, ISoftDelete
    {
        public int TenantId { get; set; }
        public Customer Customer { get; set; }
        public long CustomerId { get; set; }
        public AddressType Type { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public bool IsDeleted { get; set; }
    }
}
