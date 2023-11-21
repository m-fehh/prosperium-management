using Abp.Application.Services;
using Prosperium.Management.OpenAPI.V1.Tags.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prosperium.Management.OpenAPI.V1.Tags
{
    public interface ITagAppService : IApplicationService
    {
        Task<List<TagDto>> GetTagsListAsync();
    }
}
