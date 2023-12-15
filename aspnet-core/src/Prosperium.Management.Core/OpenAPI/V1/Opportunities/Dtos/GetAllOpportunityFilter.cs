using Abp.Application.Services.Dto;

namespace Prosperium.Management.OpenAPI.V1.Opportunities.Dtos
{
    public class GetAllOpportunityFilter : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public string MonthYear { get; set; }
    }
}
