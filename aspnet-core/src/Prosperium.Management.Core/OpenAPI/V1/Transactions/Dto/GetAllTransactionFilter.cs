using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosperium.Management.OpenAPI.V1.Transactions.Dto
{
    public class GetAllTransactionFilter : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
