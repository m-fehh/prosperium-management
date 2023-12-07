using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Prosperium.Management.OpenAPI.V1.Accounts.AccountConsts;

namespace Prosperium.Management.OpenAPI.V1.Customers
{
    [Table("Pxp_Customers")]
    public class Customer : Entity<long>, IMustHaveTenant, ISoftDelete
    {
        public int TenantId { get; set; }
        public string FullName { get; set; }
        public string DocumentType { get; set; }
        public string Document { get; set; }
        public string TaxNumber { get; set; }
        public string CompanyName { get; set; }
        public string JobTitle { get; set; }
        public ICollection<CustomerPhones> Phones { get; set; }
        public ICollection<CustomerEmails> Emails { get; set; }
        public ICollection<CustomerAddresses> Addresses { get; set; }
        public AccountOrigin Origin { get; set; }
        public string PluggyIdentityId { get; set; }
        public string PluggyItemId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
