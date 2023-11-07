using Abp.Application.Services;
using Prosperium.Management.OpenAPI.V1.Categories.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prosperium.Management.OpenAPI.V1.Categories
{
    public interface ICategoryAppService : IApplicationService
    {
        Task CreateAsync(CategoryDto input);
        Task<CategoryDto> GetByIdAsync(long id);
        Task<List<CategoryDto>> GetAllAsync();
        Task DeleteAsync(long id);
    }
}
