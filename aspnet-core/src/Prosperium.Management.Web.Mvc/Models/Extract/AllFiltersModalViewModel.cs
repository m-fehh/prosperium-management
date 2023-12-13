using Microsoft.AspNetCore.Mvc.Rendering;
using Prosperium.Management.OpenAPI.V1.Accounts.Dto;
using Prosperium.Management.OpenAPI.V1.Tags.Dto;
using Prosperium.Management.OpenAPI.V1.Categories.Dto;
using Prosperium.Management.OpenAPI.V1.CreditCards.Dto;
using System.Collections.Generic;

namespace Prosperium.Management.Web.Models.Extract
{
    public class AllFiltersModalViewModel
    {
        public List<AccountFinancialDto> Accounts { get; set; }
        public List<CreditCardDto> Cards { get; set; }
        public List<CategoryDto> Categories { get; set; }
    }
}
