using Abp.Application.Services;
using Prosperium.Management.OpenAPI.V1.Subcategories.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prosperium.Management.OpenAPI.V1.Subcategories
{
    public interface ISubcategoryAppService : IApplicationService
    {
        Task CreateAsync(SubcategoryDto input);
        Task<SubcategoryDto> GetByIdAsync(long id);
        Task<List<SubcategoryDto>> GetByCategoryIdAsync(long categoryId);
        Task<List<SubcategoryDto>> GetAllAsync();
        Task DeleteAsync(long id);
    }
}
