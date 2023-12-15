using Abp.Domain.Entities;
using Prosperium.Management.OpenAPI.V1.Accounts;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Prosperium.Management.ExternalServices.Pluggy.PluggyConsts;
using static Prosperium.Management.OpenAPI.V1.Accounts.AccountConsts;

namespace Prosperium.Management.OpenAPI.V1.Opportunities
{
    [Table("Pxp_Opportunities")]
    public class Opportunity : Entity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public long AccountId { get; set; }
        [ForeignKey("AccountId")]
        public AccountFinancial Account { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int TotalQuotas { get; set; }
        public OpportunitiesDateType QuotasType { get; set; }
        public int InterestRate { get; set; }
        public OpportunitiesType Type { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AvailableLimit { get; set; }
        public AccountOrigin Origin { get; set; }
        public string PluggyOpportunityId { get; set; }
        public string PluggyItemId { get; set; }
    }
}
