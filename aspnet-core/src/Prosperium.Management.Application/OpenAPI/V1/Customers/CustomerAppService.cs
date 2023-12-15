using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prosperium.Management.ExternalServices.Pluggy;
using Prosperium.Management.ExternalServices.Pluggy.Dtos;
using Prosperium.Management.OpenAPI.V1.Customers.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;
using static Prosperium.Management.OpenAPI.V1.Customers.CustomerConsts;

namespace Prosperium.Management.OpenAPI.V1.Customers
{
    [Route("v1/customers")]
    public class CustomerAppService : ManagementAppServiceBase, ICustomerAppService
    {
        private readonly IRepository<Customer, long> _customerRepository;
        private readonly PluggyManager _pluggyManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public CustomerAppService(IRepository<Customer, long> customerRepository, PluggyManager pluggyManager, IUnitOfWorkManager unitOfWorkManager)
        {
            _customerRepository = customerRepository;
            _pluggyManager = pluggyManager;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<List<CustomerDto>> GetAllListAsync()
        {
            List<Customer> allCustomers = await _customerRepository.GetAll()
                .Include(x => x.Addresses)
                .Include(x => x.Emails)
                .Include(x => x.Phones)
                .ToListAsync();

            return ObjectMapper.Map<List<CustomerDto>>(allCustomers);
        }


        #region Pluggy API 

        [HttpPost]
        [Route("PluggyCreateCustomer")]
        public async Task PluggyCreateCustomer(string itemId)
        {
            List<CustomerDto> customersAlreadysaved = await GetAllListAsync();
            ResultPluggyIdentity customerByPluggy = await _pluggyManager.PluggyGetIdentityAsync(itemId);

            bool isItemAlreadySaved = customersAlreadysaved
                .Any(x => x.PluggyItemId == customerByPluggy.ItemId || x.Document == customerByPluggy.Document && x.FullName.ToLower() == customerByPluggy.FullName.ToLower());

            if (!isItemAlreadySaved)
            {
                CustomerDto customerDto = new CustomerDto
                {
                    FullName = customerByPluggy.FullName,
                    DocumentType = customerByPluggy.DocumentType,
                    Document = FormatDocument(customerByPluggy.DocumentType, customerByPluggy.Document),
                    TaxNumber = customerByPluggy.TaxNumber,
                    CompanyName = customerByPluggy.CompanyName,
                    JobTitle = customerByPluggy.JobTitle,

                    Phones = customerByPluggy.PhoneNumbers?.Select(phone => new CustomerPhonesDto
                    {
                        Type = Enum.Parse<PhoneNumberType>(phone.Type),
                        PhoneNumber = phone.Value
                    }).ToList(),

                    Emails = customerByPluggy.Emails?.Select(email => new CustomerEmailsDto
                    {
                        Type = Enum.Parse<EmailType>(email.Type),
                        Email = email.Value
                    }).ToList(),

                    Addresses = customerByPluggy.Addresses?.Select(address => new CustomerAddressesDto
                    {
                        Type = Enum.Parse<AddressType>(address.Type),
                        Address = address.FullAddress,
                        City = address.City,
                        State = address.State,
                        Country = address.Country,
                        PostalCode = address.PostalCode
                    }).ToList(),

                    Origin = Accounts.AccountConsts.AccountOrigin.Pluggy,
                    PluggyIdentityId = customerByPluggy.Id,
                    PluggyItemId = itemId
                };

                Customer toInsert = ObjectMapper.Map<Customer>(customerDto);

                using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                {
                    await _customerRepository.InsertAsync(toInsert);
                    uow.Complete();
                }
            }
        }

        #endregion

        static string FormatDocument(string type, string document)
        {
            switch (type.ToUpper())
            {
                case "CPF":
                    string numericCPF = new string(document.Where(char.IsDigit).ToArray());
                    return Convert.ToUInt64(numericCPF).ToString(@"000\.000\.000\-00");
                case "CNPJ":
                    string numericCNPJ = new string(document.Where(char.IsDigit).ToArray());
                    return Convert.ToUInt64(numericCNPJ).ToString(@"00\.000\.000\/0000\-00");
                case "RG":
                    return Regex.Replace(document, @"^(\d{2})(\d{3})(\d{3})(\d{1,2}).*", "$1.$2.$3-$4");
                default:
                    return document;
            }
        }
    }
}
