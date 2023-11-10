using Abp.Application.Services;
using Prosperium.Management.OpenAPI.V1.Categories.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Prosperium.Management.OpenAPI.V1.Transactions.TransactionConsts;

namespace Prosperium.Management.OpenAPI.V1.Categories
{
    public interface ICategoryAppService : IApplicationService
    {
        Task CreateAsync(CategoryDto input);
        Task<CategoryDto> GetByIdAsync(long id);
        Task<List<CategoryDto>> GetAllAsync(TransactionType transactionType);
        Task DeleteAsync(long id);
    }
}
