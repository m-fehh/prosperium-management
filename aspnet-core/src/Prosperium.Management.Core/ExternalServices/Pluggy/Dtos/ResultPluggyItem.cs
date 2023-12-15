using System;

namespace Prosperium.Management.ExternalServices.Pluggy.Dtos
{
    public class ResultPluggyItem
    {
        public string Id { get; set; }
        public PluggyConnectorDto Connector { get; set; }
        public string Status { get; set; }
        public string ExecutionStatus { get; set; }
        public DateTime? NextAutoSyncAt { get; set; }
        public UserAction UserAction { get; set; }
    }

    public class UserAction
    {
        public string Type { get; set; }
        public string Instructions { get; set; }
        public string ExpiresAt { get; set; }
        public Attribute Attributes { get; set; }
    }

    public class Attribute
    {
        public string Name { get; set; }
        public string Data { get; set; }
    }
}
