using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Prosperium.Management.Banks;
using System;
using static Prosperium.Management.OpenAPI.V1.Accounts.AccountConsts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Prosperium.Management.OpenAPI.V1.Transactions.Dto;
using System.Collections.Generic;

namespace Prosperium.Management.OpenAPI.V1.Accounts.Dto
{
    public class AccountFinancialDto : AuditedEntityDto<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual long BankId { get; set; }
        public virtual BankDto Bank { get; set; }
        public string AccountNickname { get; set; }
        public string AgencyNumber { get; set; }
        public string AccountNumber { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal BalanceAvailable { get; set; }
        public AccountType AccountType { get; set; }
        public bool MainAccount { get; set; }

        // Propriedade de navegação para a transação
        public ICollection<CreateTransactionDto> Transaction { get; set; }
    }
}
