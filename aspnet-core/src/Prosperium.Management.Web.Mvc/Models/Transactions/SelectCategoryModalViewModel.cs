using Prosperium.Management.OpenAPI.V1.Categories.Dto;
using Prosperium.Management.OpenAPI.V1.Subcategories;
using System.Collections.Generic;

namespace Prosperium.Management.Web.Models.Transactions
{
    public class SelectCategoryModalViewModel
    {
        public List<CategoryDto> Categories { get; set; }
    }
}
