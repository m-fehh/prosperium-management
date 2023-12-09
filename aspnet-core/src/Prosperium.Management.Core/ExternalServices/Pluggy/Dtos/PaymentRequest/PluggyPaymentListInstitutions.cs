using System;
using System.Collections.Generic;

namespace Prosperium.Management.ExternalServices.Pluggy.Dtos.PaymentRequest
{
    public class PluggyPaymentListInstitutions
    {
        public int? Total { get; set; }
        public int? TotalPages { get; set; }
        public int? Page { get; set; }
        public List<PluggyInstitutionDto> Results { get; set; }
    }

    public class PluggyInstitutionDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string TradeName { get; set; }
        public string Ispb { get; set; }
        public string Compe { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
