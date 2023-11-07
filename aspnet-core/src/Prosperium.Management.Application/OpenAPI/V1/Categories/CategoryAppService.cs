using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prosperium.Management.OpenAPI.V1.Categories.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Prosperium.Management.OpenAPI.V1.Categories
{
    [Route("v1/categories")]
    public class CategoryAppService : ManagementAppServiceBase, ICategoryAppService
    {
        private readonly IRepository<Category, long> _categoryRepository;

        public CategoryAppService(IRepository<Category, long> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpPost]
        public async Task CreateAsync(CategoryDto input)
        {
            Category category = ObjectMapper.Map<Category>(input);
            await _categoryRepository.InsertAsync(category);
        }

        [HttpGet("{id}")]
        public async Task<CategoryDto> GetByIdAsync(long id)
        {
            Category category = await _categoryRepository.GetAsync(id);

            if (category == null)
            {
                throw new UserFriendlyException("A categoria não foi encontrada.");
            }

            return ObjectMapper.Map<CategoryDto>(category);
        }

        [HttpGet]
        public async Task<List<CategoryDto>> GetAllAsync()
        {
            List<Category> allCategories = await _categoryRepository.GetAll()
                .Where(c => c.TenantId == AbpSession.TenantId.Value).ToListAsync();

            return ObjectMapper.Map<List<CategoryDto>>(allCategories);
        }

        [HttpDelete("id")]
        public async Task DeleteAsync(long id)
        {
            Category searchCategory = await _categoryRepository.FirstOrDefaultAsync(id);

            if (searchCategory == null)
            {
                throw new UserFriendlyException("A categoria não foi encontrada.");
            }

            await _categoryRepository.DeleteAsync(searchCategory);
        }
    }
}
