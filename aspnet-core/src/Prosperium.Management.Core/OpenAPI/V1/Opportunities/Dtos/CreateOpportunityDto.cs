using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Prosperium.Management.ExternalServices.Pluggy.PluggyConsts;
using static Prosperium.Management.OpenAPI.V1.Accounts.AccountConsts;

namespace Prosperium.Management.OpenAPI.V1.Opportunities.Dtos
{
    public class CreateOpportunityDto : EntityDto<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual long? AccountId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public int? TotalQuotas { get; set; }
        public OpportunitiesDateType QuotasType { get; set; }
        public int? InterestRate { get; set; }
        public OpportunitiesType Type { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AvailableLimit { get; set; }
        public AccountOrigin Origin { get; set; }
        public string PluggyOpportunityId { get; set; }
        public string PluggyItemId { get; set; }
    }
}
