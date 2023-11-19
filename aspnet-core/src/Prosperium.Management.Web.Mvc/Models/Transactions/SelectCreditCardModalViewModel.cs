using Prosperium.Management.OpenAPI.V1.CreditCards.Dto;
using System.Collections.Generic;

namespace Prosperium.Management.Web.Models.Transactions
{
    public class SelectCreditCardModalViewModel
    {
        public List<CreditCardDto> Cards { get; set; }
    }
}
