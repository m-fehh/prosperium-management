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

        [Range(1, 31, ErrorMessage = "O dia de vencimento deve estar entre 1 e 31.")]
        public int DueDayInput { get; set; }

        // Propriedade de string para exibição formatada
        public string DueDay => DueDayInput.ToString("D2");
    }
}
