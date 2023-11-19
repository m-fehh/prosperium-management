using Abp.Domain.Entities;
using System;
using Abp.Application.Services.Dto;
using Prosperium.Management.OpenAPI.V1.Accounts.Dto;
using Prosperium.Management.OpenAPI.V1.Flags.Dto;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Prosperium.Management.OpenAPI.V1.CreditCards.Dto
{
    public class CreditCardDto : AuditedEntityDto<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string CardName { get; set; }
        public virtual long AccountId { get; set; }
        public AccountFinancialDto Account { get; set; }
        public virtual long FlagCardId { get; set; }
        public FlagCardDto FlagCard { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Limit { get; set; }

        public int DueDay { get; set; }

        // Propriedade de string para exibição formatada
        public string DueDayInput => DueDay.ToString("D2");
    }
}
