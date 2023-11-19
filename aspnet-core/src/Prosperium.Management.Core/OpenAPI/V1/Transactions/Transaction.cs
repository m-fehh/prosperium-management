using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Prosperium.Management.OpenAPI.V1.Accounts;
using Prosperium.Management.OpenAPI.V1.Categories;
using Prosperium.Management.OpenAPI.V1.CreditCards;
using Prosperium.Management.OpenAPI.V1.Tags;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Prosperium.Management.OpenAPI.V1.Transactions.TransactionConsts;

namespace Prosperium.Management.OpenAPI.V1.Transactions
{
    [Table("Pxp_Transactions")]
    public class Transaction : AuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public TransactionType TransactionType { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ExpenseValue { get; set; }
        public string Description { get; set; }
        public virtual long CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Categories { get; set; }
        public PaymentType PaymentType { get; set; }
        public PaymentTerms PaymentTerm { get; set; }

        public virtual long? AccountId { get; set; }

        [ForeignKey("AccountId")]
        public AccountFinancial Account { get; set; }
        public virtual int? Installments { get; set; }
        public virtual string CurrentInstallment { get; set; }

        public virtual long? CreditCardId { get; set; }

        [ForeignKey("CreditCardId")]
        public CreditCard CreditCard { get; set; }
        public DateTime Date { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}
