using Prosperium.Management.OpenAPI.V1.Categories.Dto;
using System.Collections.Generic;

namespace Prosperium.Management.Web.Models.OriginDestinations
{
    public class OriginDestinationModalViewModel
    {
        public List<CategoryDto> Categories { get; set; }
        public int PluggyId { get; set; }
    }
}
