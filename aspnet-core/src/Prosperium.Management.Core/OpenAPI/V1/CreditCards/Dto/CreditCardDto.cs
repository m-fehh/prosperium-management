using Abp.Domain.Entities;
using System;
using Abp.Application.Services.Dto;
using Prosperium.Management.OpenAPI.V1.Accounts.Dto;
using Prosperium.Management.OpenAPI.V1.Flags.Dto;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Prosperium.Management.OpenAPI.V1.Transactions.Dto;
using Newtonsoft.Json;
using static Prosperium.Management.OpenAPI.V1.Accounts.AccountConsts;

namespace Prosperium.Management.OpenAPI.V1.CreditCards.Dto
{
    public class CreditCardDto : AuditedEntityDto<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public virtual long? AccountId { get; set; }
        [JsonIgnore]
        public AccountFinancialDto Account { get; set; }
        public virtual long FlagCardId { get; set; }
        public FlagCardDto FlagCard { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Limit { get; set; }

        public int DueDay { get; set; }
        public string DueDayInput => DueDay.ToString("D2");
        [JsonIgnore]
        public ICollection<TransactionDto> Transactions { get; set; }
        public bool IsActive { get; set; }
        public AccountOrigin Origin { get; set; }
        public string PluggyItemId { get; set; }
        public string PluggyCreditCardId { get; set; }
    }
}
