using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Prosperium.Management.OpenAPI.V1.Transactions.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prosperium.Management.OpenAPI.V1.Transactions
{
    public interface ITransactionAppService : IApplicationService
    {
        Task CreateAsync(CreateTransactionDto input);
        Task<TransactionDto> GetByIdAsync(long id);
        Task<List<TransactionDto>> GetAllListAsync();
        Task<List<TransactionDto>> GetAllTransactionPerAccount(long accountId);
        Task<PagedResultDto<GetTransactionForViewDto>> GetAllAsync(GetAllTransactionFilter input);
        Task UpdateAsync(TransactionDto input);
        Task DeleteAsync(long id);
        Task CapturePluggyTransactionsAsync(string accountId, DateTime? dateInitial, DateTime? dateEnd);
    }
}
