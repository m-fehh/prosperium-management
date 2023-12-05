using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Prosperium.Management.ExternalServices.Pluggy.Dtos.PaymentRequest
{
    public class PluggyPaymentRequestInput
    {
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }
        public Recipient Recipient { get; set; }
        public string ClientPaymentId { get; set; }
    }

    public class Recipient
    {
        public string TaxNumber { get; set; }
        public string Name { get; set; }
        public Account Account { get; set; }
    }

    public class Account
    {
        public string Ispb { get; set; }
        public string Branch { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }
    }
}
