using Abp.Application.Services.Dto;
using System;

namespace Prosperium.Management.OpenAPI.V1.Transactions.Dto
{
    public class GetAllTransactionFilter : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public string MonthYear { get; set; }
    }
}
