using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Prosperium.Management.ExternalServices.Pluggy.Dtos.PaymentRequest
{
    public class PluggyPaymentRequestOutput
    {
        public string Id { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string ClientPaymentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Recipient Recipient { get; set; }
        public string PaymentUrl { get; set; }
    }
}
