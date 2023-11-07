using Abp.Application.Services.Dto;

namespace Prosperium.Management.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}

