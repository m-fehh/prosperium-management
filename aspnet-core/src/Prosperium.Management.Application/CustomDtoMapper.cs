using AutoMapper;
using Prosperium.Management.Authorization.Roles;
using Prosperium.Management.Authorization.Users;
using Prosperium.Management.OpenAPI.V1.Accounts;
using Prosperium.Management.OpenAPI.V1.Accounts.Dto;
using Prosperium.Management.OpenAPI.V1.Banks;
using Prosperium.Management.OpenAPI.V1.Banks.Dtos;
using Prosperium.Management.OpenAPI.V1.Categories;
using Prosperium.Management.OpenAPI.V1.Categories.Dto;
using Prosperium.Management.OpenAPI.V1.CreditCards;
using Prosperium.Management.OpenAPI.V1.CreditCards.Dto;
using Prosperium.Management.OpenAPI.V1.Customers;
using Prosperium.Management.OpenAPI.V1.Customers.Dtos;
using Prosperium.Management.OpenAPI.V1.Flags;
using Prosperium.Management.OpenAPI.V1.Flags.Dto;
using Prosperium.Management.OpenAPI.V1.Opportunities;
using Prosperium.Management.OpenAPI.V1.Opportunities.Dtos;
using Prosperium.Management.OpenAPI.V1.Subcategories;
using Prosperium.Management.OpenAPI.V1.Subcategories.Dto;
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
            configuration.CreateMap<Bank, BankDto>().ReverseMap();
            configuration.CreateMap<AccountFinancial, AccountFinancialDto>().ReverseMap();
            configuration.CreateMap<CreditCard, CreditCardDto>().ReverseMap();
            configuration.CreateMap<CreditCard, CreateCreditCardDto>().ReverseMap();
            configuration.CreateMap<FlagCard, FlagCardDto>().ReverseMap();
            configuration.CreateMap<Opportunity, OpportunityDto>().ReverseMap();
            configuration.CreateMap<Opportunity, CreateOpportunityDto>().ReverseMap();

            configuration.CreateMap<Role, RoleDto>().ReverseMap();
            configuration.CreateMap<User, UserDto>().ReverseMap();

            configuration.CreateMap<Customer, CustomerDto>().ReverseMap();
            configuration.CreateMap<CustomerPhones, CustomerPhonesDto>().ReverseMap();
            configuration.CreateMap<CustomerEmails, CustomerEmailsDto>().ReverseMap();
            configuration.CreateMap<CustomerAddresses, CustomerAddressesDto>().ReverseMap();
        }
    }
}
