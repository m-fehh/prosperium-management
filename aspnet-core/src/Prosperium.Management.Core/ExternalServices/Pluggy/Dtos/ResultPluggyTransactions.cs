using System;
using System.Collections.Generic;

namespace Prosperium.Management.ExternalServices.Pluggy.Dtos
{
    public class ResultPluggyTransactions
    {
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public int Page { get; set; }
        public List<PluggyTransactionDto> Results { get; set; }
    }

    public class PluggyTransactionDto
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string DescriptionRaw { get; set; }
        public string CurrencyCode { get; set; }
        public string Amount { get; set; }
        public DateTime Date { get; set; }
        public string Balance { get; set; }
        public string Category { get; set; }
        public string AccountId { get; set; }
        public string ProviderCode { get; set; }
        public string Status { get; set; }
        public PaymentData PaymentData { get; set; }
    }

    public class PaymentData
    {
        public string Reason { get; set; }
        public string PaymentMethod { get; set; }
        public string ReferenceNumber { get; set; }
    }
}
