using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prosperium.Management.OpenAPI.V1.Subcategories.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prosperium.Management.OpenAPI.V1.Subcategories
{
    [Route("v1/subcategory")]
    public class SubcategoryAppService : ManagementAppServiceBase, ISubcategoryAppService
    {
        private readonly IRepository<Subcategory, long> _subcategoryRepository;

        public SubcategoryAppService(IRepository<Subcategory, long> subcategoryRepository)
        {
            _subcategoryRepository = subcategoryRepository;
        }

        [HttpPost]
        public async Task CreateAsync(SubcategoryDto input)
        {
            Subcategory subcategory = ObjectMapper.Map<Subcategory>(input);
            await _subcategoryRepository.InsertAsync(subcategory);
        }

        [HttpGet("{id}")]
        public async Task<SubcategoryDto> GetByIdAsync(long id)
        {
            Subcategory subcategory = await _subcategoryRepository.GetAsync(id);

            if (subcategory == null)
            {
                throw new UserFriendlyException("A sub-categoria não foi encontrada.");
            }

            return ObjectMapper.Map<SubcategoryDto>(subcategory);
        }

        [HttpGet("{categoryId}")]
        public async Task<List<SubcategoryDto>> GetByCategoryIdAsync(long categoryId)
        {
            List<Subcategory> subcategories = await _subcategoryRepository.GetAll().Include(x => x.Category).Where(x => x.Category.Id == categoryId).ToListAsync();

            return ObjectMapper.Map<List<SubcategoryDto>>(subcategories);
        }

        [HttpGet]
        public async Task<List<SubcategoryDto>> GetAllAsync()
        {
            List<Subcategory> allSubcategories = await _subcategoryRepository.GetAll()
                .Include(x => x.Category).ToListAsync();

            return ObjectMapper.Map<List<SubcategoryDto>>(allSubcategories);
        }

        [HttpDelete("id")]
        public async Task DeleteAsync(long id)
        {
            Subcategory searchSubcategory = await _subcategoryRepository.FirstOrDefaultAsync(id);

            if (searchSubcategory == null)
            {
                throw new UserFriendlyException("A sub-categoria não foi encontrada.");
            }

            await _subcategoryRepository.DeleteAsync(searchSubcategory);
        }
    }
}
