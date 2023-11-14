using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Prosperium.Management.Banks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Prosperium.Management.OpenAPI.V1.Accounts.AccountConsts;

namespace Prosperium.Management.OpenAPI.V1.Accounts
{
    [Table("Pxp_Accounts")]
    public class AccountFinancial : AuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Bank Institution { get; set; }
        public string AccountNickname { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal BalanceAvailable { get; set; } // Saldo disponível na conta
        public AccountType AccountType { get; set; }
        public bool MainAccount { get; set; }
    }
}
