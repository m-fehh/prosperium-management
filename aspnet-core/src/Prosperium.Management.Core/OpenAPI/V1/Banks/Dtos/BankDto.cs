using Abp.Application.Services.Dto;
using static Prosperium.Management.OpenAPI.V1.Accounts.AccountConsts;

namespace Prosperium.Management.OpenAPI.V1.Banks.Dtos
{
    public class BankDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public AccountOrigin Origin { get; set; }
    }
}
