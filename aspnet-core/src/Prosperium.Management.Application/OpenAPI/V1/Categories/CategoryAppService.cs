using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Prosperium.Management.ExternalServices.Pluggy;
using Prosperium.Management.ExternalServices.Pluggy.Dtos;
using Prosperium.Management.ExternalServices.Pluggy.Models;
using Prosperium.Management.OpenAPI.V1.Categories.Dto;
using Prosperium.Management.OpenAPI.V1.Transactions;
using Prosperium.Management.OpenAPI.V1.Transactions.Dto;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly IRepository<PluggyCategory> _pluggyCategoriesRepository;
        private readonly PluggyManager _pluggyManager;

        public CategoryAppService(IRepository<Category, long> categoryRepository, IRepository<Transaction, long> transactionRepository, IRepository<PluggyCategory> pluggyCategoriesRepository, PluggyManager pluggyManager)
        {
            _categoryRepository = categoryRepository;
            _transactionRepository = transactionRepository;
            _pluggyCategoriesRepository = pluggyCategoriesRepository;
            _pluggyManager = pluggyManager;
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

        #region Pluggy 

        [HttpGet]
        [Route("GetCategoriesPluggy")]
        public async Task GetCategoriesPluggy()
        {
            #region Tradução 

            CultureInfo culturaOrigem = new CultureInfo("en");
            CultureInfo culturaDestino = new CultureInfo("pt");

            #endregion

            ResultPluggyCategories categories = await _pluggyManager.PluggyGetCategories();

            List<PluggyCategory> InsertPluggyCategories = new List<PluggyCategory>();

            if (categories.Total > 0)
            {
                foreach (var category in categories.Results)
                {
                    if (category.ParentDescription != null)
                    {
                        category.ParentDescriptionTranslated = culturaDestino.TextInfo.ToTitleCase(await TranslateTextAsync(category.ParentDescription, "en", "pt"));
                    }

                    InsertPluggyCategories.Add(new PluggyCategory
                    {
                        Pluggy_Id = category.Id,
                        Pluggy_Category_Id = category.ParentId,
                        Pluggy_Category_Name = category.ParentDescription,
                        Pluggy_Category_Name_Translated = category.ParentDescriptionTranslated,
                        Pluggy_Description = category.Description,
                        Pluggy_Description_Translated = category.DescriptionTranslated
                    });
                }

                await _pluggyCategoriesRepository.InsertRangeAsync(InsertPluggyCategories);
            }
        }

        public async Task<string> TranslateTextAsync(string text, string fromLanguage, string toLanguage)
        {
            string endpoint = $"https://translate.googleapis.com/translate_a/single";

            var result = await endpoint
                .SetQueryParam("client", "gtx")
                .SetQueryParam("sl", fromLanguage)
                .SetQueryParam("tl", toLanguage)
                .SetQueryParam("dt", "t")
                .SetQueryParam("q", text)
                .GetJsonAsync<JArray>();

            string translation = result?[0]?.FirstOrDefault()?.FirstOrDefault()?.ToString();

            return translation;
        }

        [HttpGet]
        [Route("GetAllCategoriesPluggyAsync")]
        public async Task<PagedResultDto<GetCategoriesForViewDto>> GetAllCategoriesPluggyAsync(GetAllCategoriesFilter input)
        {
            var categoriesPluggy = _pluggyCategoriesRepository.GetAll()
                .Where(x => !string.IsNullOrEmpty(x.Pluggy_Category_Name) && string.IsNullOrEmpty(x.Prosperium_Category_Id.ToString()));

            var pagedAndFilteredCategories = categoriesPluggy.OrderBy(input.Sorting ?? "id asc").PageBy(input);

            var categories = from o in pagedAndFilteredCategories
                             select new GetCategoriesForViewDto()
                               {
                                   Category = ObjectMapper.Map<CategoryPluggyDto>(o),
                               };

            int totalCount = await categoriesPluggy.CountAsync();
            return new PagedResultDto<GetCategoriesForViewDto>
            (
                totalCount,
                await categories.ToListAsync()
            );
        }

        [HttpPut]
        [Route("UpdateCategoryPluggy")]
        public async Task UpdateCategoryPluggyAsync(int pluggyId, long categoryProsperiumId)
        {
            var pluggyCategory = await _pluggyCategoriesRepository.GetAllListAsync();

            if (pluggyCategory != null)
            {
                var categoryName = pluggyCategory.Where(x => x.Id == pluggyId).Select(x => x.Pluggy_Category_Name).FirstOrDefault();

                var categoriesToUpdate = pluggyCategory.Where(c => c.Pluggy_Category_Name == categoryName).ToList();

                foreach (var categoryToUpdate in categoriesToUpdate)
                {
                    categoryToUpdate.Prosperium_Category_Id = categoryProsperiumId;
                    await _pluggyCategoriesRepository.UpdateAsync(categoryToUpdate);
                }
            }
        }
        #endregion
    }
}