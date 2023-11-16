﻿using AutoMapper;
using Prosperium.Management.Authorization.Roles;
using Prosperium.Management.Authorization.Users;
using Prosperium.Management.Banks;
using Prosperium.Management.OpenAPI.V1.Accounts;
using Prosperium.Management.OpenAPI.V1.Accounts.Dto;
using Prosperium.Management.OpenAPI.V1.Categories;
using Prosperium.Management.OpenAPI.V1.Categories.Dto;
using Prosperium.Management.OpenAPI.V1.Subcategories;
using Prosperium.Management.OpenAPI.V1.Subcategories.Dto;
using Prosperium.Management.OpenAPI.V1.Tags;
using Prosperium.Management.OpenAPI.V1.Tags.Dto;
using Prosperium.Management.OpenAPI.V1.Transactions;
using Prosperium.Management.OpenAPI.V1.Transactions.Dto;
using Prosperium.Management.Roles.Dto;
using Prosperium.Management.Users.Dto;

namespace PayMetrix
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        { 
            configuration.CreateMap<Category, CategoryDto>().ReverseMap();
            configuration.CreateMap<Subcategory, SubcategoryDto>().ReverseMap();
            configuration.CreateMap<Transaction, TransactionDto>().ReverseMap();
            configuration.CreateMap<Transaction, CreateTransactionDto>().ReverseMap();
            configuration.CreateMap<Tag, TagDto>().ReverseMap();
            configuration.CreateMap<Bank, BankDto>().ReverseMap();
            configuration.CreateMap<AccountFinancial, AccountFinancialDto>().ReverseMap();

            configuration.CreateMap<Role, RoleDto>().ReverseMap();
            configuration.CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
