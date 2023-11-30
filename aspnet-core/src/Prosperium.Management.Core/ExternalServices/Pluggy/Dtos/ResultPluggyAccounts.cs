using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Prosperium.Management.ExternalServices.Pluggy.Dtos
{
    public class ResultPluggyAccounts
    {
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public int Page { get; set; }
        public List<PluggyAccountsDto> Results { get; set; }
    }

    public class PluggyAccountsDto
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Subtype { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
        public string CurrencyCode { get; set; }
        public string ItemId { get; set; }
        public string Number { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string MarketingName { get; set; }
        public string TaxNumber { get; set; }
        public string Owner { get; set; }
        public List<BankData> BankData { get; set; }
        public List<CreditData> CreditData { get; set; }
    }

    public class BankData 
    { 
        public string TransferNumber { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ClosingBalance { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AutomaticallyInvestedBalance { get; set; }
    
    }
    public class CreditData 
    { 
        public string Level { get; set; }
        public string Brand { get; set; }
        public string BalanceDueDate { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AvailableCreditLimit { get; set; }
        public string Status { get; set; }
    
    
    }
}
