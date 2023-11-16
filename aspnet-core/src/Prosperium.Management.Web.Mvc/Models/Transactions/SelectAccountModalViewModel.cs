using Prosperium.Management.OpenAPI.V1.Accounts.Dto;
using System.Collections.Generic;

namespace Prosperium.Management.Web.Models.Transactions
{
    public class SelectAccountModalViewModel
    {
        public List<AccountFinancialDto> Accounts { get; set; }
    }
}
