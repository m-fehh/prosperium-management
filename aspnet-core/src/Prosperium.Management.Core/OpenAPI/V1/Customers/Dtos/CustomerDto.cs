using System.Collections.Generic;
using static Prosperium.Management.OpenAPI.V1.Accounts.AccountConsts;
using static Prosperium.Management.OpenAPI.V1.Customers.CustomerConsts;

namespace Prosperium.Management.OpenAPI.V1.Customers.Dtos
{
    public class CustomerDto
    {
        public int TenantId { get; set; }
        public string FullName { get; set; }
        public string DocumentType { get; set; }
        public string Document { get; set; }
        public string TaxNumber { get; set; }
        public string CompanyName { get; set; }
        public string JobTitle { get; set; }
        public AccountOrigin Origin { get; set; }
        public string PluggyIdentityId { get; set; }
        public string PluggyItemId { get; set; }

        public List<CustomerPhonesDto> Phones { get; set; }
        public List<CustomerEmailsDto> Emails { get; set; }
        public List<CustomerAddressesDto> Addresses { get; set; }
    }

    public class CustomerPhonesDto
    {
        public int TenantId { get; set; }
        public PhoneNumberType Type { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class CustomerAddressesDto
    {
        public int TenantId { get; set; }
        public AddressType Type { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
    }

    public class CustomerEmailsDto
    {
        public int TenantId { get; set; }
        public EmailType Type { get; set; }
        public string Email { get; set; }
    }
}
