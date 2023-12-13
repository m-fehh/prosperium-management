using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prosperium.Management.ExternalServices.Pluggy.Dtos
{
    public class ResultPluggyAccounts
    {
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public int Page { get; set; }
        public List<PluggyAccount> Results { get; set; }
    }

    public class PluggyAccount
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Subtype { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal?(18,2)")]
        public decimal? Balance { get; set; }

        public string CurrencyCode { get; set; }
        public string ItemId { get; set; }
        public string Number { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string MarketingName { get; set; }
        public string TaxNumber { get; set; }
        public string Owner { get; set; }
        public BankData BankData { get; set; }
        public CreditData CreditData { get; set; }
    }

    public class BankData
    {
        public string TransferNumber { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal?(18,2)")]
        public decimal? ClosingBalance { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal?(18,2)")]
        public decimal? AutomaticallyInvestedBalance { get; set; }
    }

    public class CreditData
    {
        public string Level { get; set; }
        public string Brand { get; set; }
        public DateTime BalanceCloseDate { get; set; }
        public DateTime BalanceDueDate { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal?(18,2)")]
        public decimal? AvailableCreditLimit { get; set; }

        public decimal? BalanceForeignCurrency { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal?(18,2)")]
        public decimal? MinimumPayment { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal?(18,2)")]
        public decimal? CreditLimit { get; set; }

        public bool? IsLimitFlexible { get; set; }
        public string HolderType { get; set; }
        public string Status { get; set; }
    }
}

