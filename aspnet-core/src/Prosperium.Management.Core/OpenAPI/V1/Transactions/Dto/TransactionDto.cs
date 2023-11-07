﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Prosperium.Management.OpenAPI.V1.Transactions.TransactionConsts;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;

namespace Prosperium.Management.OpenAPI.V1.Transactions.Dto
{
    public class TransactionDto : AuditedEntityDto<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public TransactionType TransactionType { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ExpenseValue { get; set; }
        public string Description { get; set; }

        // Categoria FK
        public PaymentType PaymentType { get; set; }
        public PaymentTerms PaymentTerm { get; set; }

        // CONTA abrir modal
        public DateTime Date { get; set; }
        public bool Paid { get; set; }
    }
}