using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace Prosperium.Management.OpenAPI.V1.Transactions.Dto
{
    public class GetAllTransactionFilter : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public string MonthYear { get; set; }

        public string FilteredAccounts { get; set; }
        public string filteredCards { get; set; }
        public string FilteredCategories { get; set; }
        public bool? FilteredExpense { get; set; }
        public bool? FilteredRecipes { get; set; }
    }
}
