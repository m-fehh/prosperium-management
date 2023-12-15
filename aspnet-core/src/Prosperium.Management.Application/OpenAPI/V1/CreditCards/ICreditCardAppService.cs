using Prosperium.Management.ExternalServices.Pluggy.Dtos;
using Prosperium.Management.OpenAPI.V1.CreditCards.Dto;
using Prosperium.Management.OpenAPI.V1.Flags.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prosperium.Management.OpenAPI.V1.CreditCards
{
    public interface ICreditCardAppService
    {
        Task<List<CreditCardDto>> GetAllListAsync();
        Task<List<FlagCardDto>> GetAllListFlagsAsync();
        Task DeleteAsync(long creditCardId);
        Task CreateAsync(CreateCreditCardDto input);
        Task UpdateAsync(CreditCardDto input);
    }
}
