using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Prosperium.Management.Banks;
using System;
using static Prosperium.Management.OpenAPI.V1.Accounts.AccountConsts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Prosperium.Management.OpenAPI.V1.Accounts.Dto
{
    public class AccountFinancialDto : AuditedEntityDto<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual long InstitutionId { get; set; }
        public string AccountNickname { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal BalanceAvailable { get; set; }
        public AccountType AccountType { get; set; }
        public bool MainAccount { get; set; }
    }
}
