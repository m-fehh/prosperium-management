using System;
using System.Collections.Generic;

namespace Prosperium.Management.ExternalServices.Pluggy.Dtos
{
    public class ResultPluggyConnector
    {
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public int Page { get; set; }
        public List<PluggyConnectorDto> Results { get; set; }
    }

    public class PluggyConnectorDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PrimaryColor { get; set; }
        public string InstitutionUrl { get; set; }
        public string Country { get; set; }
        public string Type { get; set; }
        public List<PluggyConnectorCredentials> Credentials { get; set; }
        public string ImageUrl { get; set; }
        public bool HasMFA { get; set; }
        public List<string> Products { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class PluggyConnectorCredentials
    {
        public string Validation { get; set; }
        public string ValidationMessage { get; set; }
        public string Label { get; set; }
    }
}
