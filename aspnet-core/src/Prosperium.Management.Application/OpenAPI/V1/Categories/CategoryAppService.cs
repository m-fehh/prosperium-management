using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Prosperium.Management.OpenAPI.V1.Categories.Dto;
using Prosperium.Management.OpenAPI.V1.Transactions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static Prosperium.Management.OpenAPI.V1.Transactions.TransactionConsts;

namespace Prosperium.Management.OpenAPI.V1.Categories
{
    [Route("v1/categories")]
    public class CategoryAppService : ManagementAppServiceBase, ICategoryAppService
    {
        private readonly IRepository<Category, long> _categoryRepository;
        private readonly IRepository<Transaction, long> _transactionRepository;

        public CategoryAppService(IRepository<Category, long> categoryRepository, IRepository<Transaction, long> transactionRepository)
        {
            _categoryRepository = categoryRepository;
            _transactionRepository = transactionRepository;
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
        [Route("list-categories")]
        public async Task<List<CategoryDto>> GetAllListAsync()
        {
            List<Category> allCategories = await _categoryRepository.GetAll().Include(x => x.Subcategories).ToListAsync();

            return ObjectMapper.Map<List<CategoryDto>>(allCategories);
        }

        [HttpGet]
        [Route("list-categories-per-tenant")]
        public async Task<List<CategoryDto>> GetAllListPerTenantAsync()
        {
            var allCategoriesForTransaction = await _transactionRepository.GetAll().Include(x => x.Categories).Select(x => x.Categories.Id).ToListAsync();

            List<Category> categories = new List<Category>();
            List<Category> allCategories = await _categoryRepository.GetAllListAsync();

            foreach (var category in allCategoriesForTransaction)
            {
                var categorySelected = allCategories.Where(x => x.Id == category).FirstOrDefault();
                if (categorySelected != null)
                {
                    if (!categories.Any(x => x.Id == categorySelected.Id))
                    {
                        categories.Add(categorySelected);

                    }
                }
            }

            return ObjectMapper.Map<List<CategoryDto>>(categories);
        }

        [HttpGet]
        public async Task<List<CategoryDto>> GetAllAsync(TransactionType transactionType)
        {
            List<Category> allCategories = await _categoryRepository.GetAll().Where(x => x.TransactionType == transactionType).ToListAsync();

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