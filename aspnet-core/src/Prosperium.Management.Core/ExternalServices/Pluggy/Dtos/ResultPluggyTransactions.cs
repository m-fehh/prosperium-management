using System;
using System.Collections.Generic;

namespace Prosperium.Management.ExternalServices.Pluggy.Dtos
{
    public class ResultPluggyTransactions
    {
        public int? Total { get; set; }
        public int? TotalPages { get; set; }
        public int? Page { get; set; }
        public List<PluggyTransactionDto> Results { get; set; }
    }

    public class PluggyTransactionDto
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string CurrencyCode { get; set; }
        public double? Amount { get; set; }
        public DateTime Date { get; set; }
        public double? Balance { get; set; }
        public string Category { get; set; }
        public string CategoryId { get; set; }
        public PaymentData PaymentData { get; set; }
        public CreditCardMetadata CreditCardMetadata { get; set; }
        public string AccountId { get; set; }
        public string ProviderCode { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string AcquirerData { get; set; }
        public string Merchant { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class PaymentData
    {
        public string Reason { get; set; }
        public string PaymentMethod { get; set; }
        public string ReferenceNumber { get; set; }
        public ReceiverData Payer { get; set; }
        public ReceiverData Receiver { get; set; }
    }

    public class ReceiverData
    {
        public string DocumentNumber { get; set; }
        public string Name { get; set; }
        public string AccountNumber { get; set; }
        public string BranchNumber { get; set; }
        public string RoutingNumber { get; set; }
        public string RoutingNumberISPB { get; set; }
    }

    public class CreditCardMetadata
    {
        public int? InstallmentNumber { get; set; }
        public int? TotalInstallments { get; set; }
        public double? TotalAmount { get; set; }
        public int? PayeeMCC { get; set; }
        public string BillId { get; set; }
        public string CardNumber { get; set; }
    }
}
