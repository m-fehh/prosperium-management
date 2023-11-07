using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Prosperium.Management.OpenAPI.V1.Transactions.Dto;
using System.Threading.Tasks;

namespace Prosperium.Management.OpenAPI.V1.Transactions
{
    public interface ITransactionAppService : IApplicationService
    {
        Task CreateAsync(TransactionDto input);
        Task<TransactionDto> GetByIdAsync(long id);
        Task<PagedResultDto<GetTransactionForViewDto>> GetAllAsync(GetAllTransactionFilter input);
        Task UpdateAsync(TransactionDto input);
        Task DeleteAsync(long id);
    }
}
