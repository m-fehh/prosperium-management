using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Prosperium.Management.OpenAPI.V1.Tags.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Prosperium.Management.OpenAPI.V1.Transactions.TransactionConsts;

namespace Prosperium.Management.OpenAPI.V1.Transactions.Dto
{
    public class CreateTransactionDto : AuditedEntityDto<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public TransactionType TransactionType { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ExpenseValue { get; set; }
        public string Description { get; set; }
        public virtual long CategoryId { get; set; }
        public PaymentType PaymentType { get; set; }
        public PaymentTerms PaymentTerm { get; set; }
        public virtual long? AccountId { get; set; }
        public virtual int? Installments { get; set; }
        public virtual long? CreditCardId { get; set; }
        public DateTime Date { get; set; }
        public ICollection<TagDto> Tags { get; set; }
    }

}
