using Microsoft.AspNetCore.Mvc.Rendering;
using Prosperium.Management.OpenAPI.V1.Accounts.Dto;
using Prosperium.Management.OpenAPI.V1.Categories.Dto;
using System.Collections.Generic;
using static Prosperium.Management.OpenAPI.V1.Transactions.TransactionConsts;

namespace Prosperium.Management.Web.Models.Extract
{
    public class AllFiltersModalViewModel
    {
        public List<AccountFinancialDto> Accounts { get; set; }
        public List<CategoryDto> Categories { get; set; }
        public List<SelectListItem> TransactionType { get; set; }
    }
}
