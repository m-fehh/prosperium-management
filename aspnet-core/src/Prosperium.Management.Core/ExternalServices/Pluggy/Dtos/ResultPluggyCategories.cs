using System.Collections.Generic;

namespace Prosperium.Management.ExternalServices.Pluggy.Dtos
{
    public class ResultPluggyCategories
    {
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public int Page { get; set; }
        public List<PluggyCategory> Results { get; set; }
    }

    public class PluggyCategory
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string DescriptionTranslated { get; set; }
        public string ParentId { get; set; }
        public string ParentDescription { get; set; }
    }
}
