using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Prosperium.Management.OpenAPI.V1.Accounts.AccountConsts;

namespace Prosperium.Management.OpenAPI.V1.CreditCards.Dto
{
    public class CreateCreditCardDto : AuditedEntityDto<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public virtual long AccountId { get; set; }
        public virtual long FlagCardId { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Limit { get; set; }
        public int DueDay { get; set; }
        public bool IsActive { get; set; }
        public AccountOrigin Origin { get; set; }
    }
}
