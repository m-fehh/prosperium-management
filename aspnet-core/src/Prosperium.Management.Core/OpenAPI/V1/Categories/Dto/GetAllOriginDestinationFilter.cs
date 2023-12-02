using Abp.Application.Services.Dto;

namespace Prosperium.Management.OpenAPI.V1.Categories.Dto
{
    public class GetAllOriginDestinationFilter : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
