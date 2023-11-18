using Abp.Application.Services.Dto;

namespace Prosperium.Management.OpenAPI.V1.Flags.Dto
{
    public class FlagCardDto : EntityDto<long>
    {
        public string IconPath { get; set; }
        public string Name { get; set; }
    }
}
