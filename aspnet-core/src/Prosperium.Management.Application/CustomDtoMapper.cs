using AutoMapper;
using Prosperium.Management.OpenAPI.V1.Categories;
using Prosperium.Management.OpenAPI.V1.Categories.Dto;
using Prosperium.Management.OpenAPI.V1.Subcategories;
using Prosperium.Management.OpenAPI.V1.Subcategories.Dto;
using Prosperium.Management.OpenAPI.V1.Transactions;
using Prosperium.Management.OpenAPI.V1.Transactions.Dto;

namespace PayMetrix
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        { 
            configuration.CreateMap<Category, CategoryDto>().ReverseMap();
            configuration.CreateMap<Subcategory, SubcategoryDto>().ReverseMap();
            configuration.CreateMap<Transaction, TransactionDto>().ReverseMap();
        }
    }
}
