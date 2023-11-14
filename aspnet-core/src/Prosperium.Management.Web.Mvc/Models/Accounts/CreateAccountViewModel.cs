using Prosperium.Management.OpenAPI.V1.Accounts.Dto;
using System.Collections.Generic;

namespace Prosperium.Management.Web.Models.Accounts
{
    public class CreateAccountViewModel
    {
        public List<BankDto> Banks { get; set; }
    }
}
