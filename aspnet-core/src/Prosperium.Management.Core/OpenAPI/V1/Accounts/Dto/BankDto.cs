using Abp.Application.Services.Dto;

namespace Prosperium.Management.OpenAPI.V1.Accounts.Dto
{
    public class BankDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
    }
}
