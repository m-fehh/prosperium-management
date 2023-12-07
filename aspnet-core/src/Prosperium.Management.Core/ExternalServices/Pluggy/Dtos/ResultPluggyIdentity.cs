using System;
using System.Collections.Generic;

namespace Prosperium.Management.ExternalServices.Pluggy.Dtos
{
    public class ResultPluggyIdentity
    {
        public string Id { get; set; }
        public string ItemId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime BirthDate { get; set; }
        public string TaxNumber { get; set; }
        public string Document { get; set; }
        public string DocumentType { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string FullName { get; set; }
        public List<PhoneNumber> PhoneNumbers { get; set; }
        public List<Email> Emails { get; set; }
        public List<Address> Addresses { get; set; }
        public List<Relation> Relations { get; set; }
        public string InvestorProfile { get; set; }
    }

    public class PhoneNumber
    {
        public string Value { get; set; }
        public string Type { get; set; }
    }

    public class Email
    {
        public string Value { get; set; }
        public string Type { get; set; }
    }

    public class Address
    {
        public string FullAddress { get; set; }
        public string State { get; set; }
        public string PrimaryAddress { get; set; }
        public string Country { get; set; }
        public string Type { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
    }

    public class Relation
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Document { get; set; }
    }
}
