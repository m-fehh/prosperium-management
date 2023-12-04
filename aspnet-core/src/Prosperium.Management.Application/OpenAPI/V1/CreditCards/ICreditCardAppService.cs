using Prosperium.Management.OpenAPI.V1.Accounts.Dto;
using Prosperium.Management.OpenAPI.V1.CreditCards.Dto;
using Prosperium.Management.OpenAPI.V1.Flags.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosperium.Management.OpenAPI.V1.CreditCards
{
    public interface ICreditCardAppService
    {
        Task<List<CreditCardDto>> GetAllListAsync();
        Task<List<FlagCardDto>> GetAllListFlagsAsync();
        Task DeleteAsync(long creditCardId);
        Task PluggyCreateCreditCard(PluggyAccount input, long accountId);
    }
}
