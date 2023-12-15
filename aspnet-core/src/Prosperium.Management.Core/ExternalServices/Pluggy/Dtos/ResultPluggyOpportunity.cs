using System;
using System.Collections.Generic;

namespace Prosperium.Management.ExternalServices.Pluggy.Dtos
{
    public class ResultPluggyOpportunity
    {
        public int? Total { get; set; }
        public int? TotalPages { get; set; }
        public int? Page { get; set; }
        public List<PluggyOpportunityDto> Results { get; set; }
    }

    public class PluggyOpportunityDto
    {
        public string Id { get; set; }
        public string ItemId { get; set; }
        public double? TotalLimit { get; set; }
        public double? UsedLimit { get; set; }
        public double? AvailableLimit { get; set; }
        public int? TotalQuotas { get; set; }
        public string QuotasType { get; set; }
        public int? InterestRate { get; set; }
        public string RateType { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
