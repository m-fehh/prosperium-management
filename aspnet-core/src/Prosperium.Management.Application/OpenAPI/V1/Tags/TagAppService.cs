using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Prosperium.Management.OpenAPI.V1.Tags.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prosperium.Management.OpenAPI.V1.Tags
{
    [Route("v1/tags")]
    public class TagAppService : ManagementAppServiceBase, ITagAppService
    {
        private readonly IRepository<Tag, long> _tagsRepository;

        public TagAppService(IRepository<Tag, long> tagsRepository)
        {
            _tagsRepository = tagsRepository;
        }

        [HttpGet]
        public async Task<List<TagDto>> GetTagsListAsync()
        {
            List<Tag> allTags = await _tagsRepository.GetAllListAsync();

            return ObjectMapper.Map<List<TagDto>>(allTags);
        }
    }
}
