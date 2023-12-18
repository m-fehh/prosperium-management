using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Repositories;
using Abp.Linq.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prosperium.Management.ExternalServices.Pluggy;
using Prosperium.Management.ExternalServices.Pluggy.Dtos;
using Prosperium.Management.OpenAPI.V1.Accounts;
using Prosperium.Management.OpenAPI.V1.Accounts.Dto;
using Prosperium.Management.OpenAPI.V1.Opportunities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Transactions;
using static Prosperium.Management.ExternalServices.Pluggy.PluggyConsts;

namespace Prosperium.Management.OpenAPI.V1.Opportunities
{
    [Route("v1/opportunities")]
    public class OpportunitiesAppService : ManagementAppServiceBase, IOpportunitiesAppService
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<Opportunity, long> _opportunityRepository;
        private readonly IAccountAppService _accountAppService;
        private readonly PluggyManager _pluggyManager;

        public OpportunitiesAppService(IUnitOfWorkManager unitOfWorkManager, IRepository<Opportunity, long> opportunityRepository, IAccountAppService accountAppService, PluggyManager pluggyManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _opportunityRepository = opportunityRepository;
            _accountAppService = accountAppService;
            _pluggyManager = pluggyManager;
        }

        [HttpGet]
        public async Task<PagedResultDto<GetOpportunityForViewDto>> GetAllAsync(GetAllOpportunityFilter input)
        {
            var query = _opportunityRepository.GetAll().Include(x => x.Account).ThenInclude(x => x.Bank).AsQueryable();


            if (!string.IsNullOrEmpty(input.MonthYear))
            {
                var parts = input.MonthYear.Split('/');
                var month = int.Parse(parts[0]);
                var year = int.Parse(parts[1]);
                query = query.Where(x => x.Date.Month == month && x.Date.Year == year);
            }

            var pagedAndFilteredOpportunities = query.OrderBy(input.Sorting ?? "date desc").PageBy(input);

            var opportunity = pagedAndFilteredOpportunities.Select(o => new GetOpportunityForViewDto
            {
                Opportunity = ObjectMapper.Map<OpportunityDto>(o),
            });

            int totalCount = await query.CountAsync();
            return new PagedResultDto<GetOpportunityForViewDto>(totalCount, await opportunity.ToListAsync());
        }

        [HttpPost]
        public async Task PluggyCreateOpportunitiesAsync(string itemId, long accountId)
        {
            ResultPluggyOpportunity opportunity = await _pluggyManager.PluggyGetOpportunitiesAsync(itemId);
            AccountFinancialDto account = (await _accountAppService.GetAllListAsync()).Where(x => x.PluggyItemId == itemId).FirstOrDefault();

            if (opportunity.Total > 0)
            {
                // Exclui os registros existentes com base no itemId
                var existingOpportunities = await _opportunityRepository
                    .GetAll()
                    .Where(x => x.PluggyItemId == itemId && x.AccountId == accountId)
                    .ToListAsync();

                if (existingOpportunities.Any())
                {
                    foreach (var existingOpportunity in existingOpportunities)
                    {
                        await _opportunityRepository.DeleteAsync(existingOpportunity.Id);
                    }
                }

                List<Opportunity> opportunitiesToInsert = new List<Opportunity>();
                foreach (var item in opportunity.Results)
                {

                    CreateOpportunityDto opportunityDto = new CreateOpportunityDto
                    {
                        AccountId = account.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Date = item.Date,
                        TotalQuotas = item.TotalQuotas ?? 0,
                        QuotasType = Enum.Parse<OpportunitiesDateType>(item.QuotasType ?? "NULL"),
                        InterestRate = item.InterestRate ?? 0,
                        Type = Enum.Parse<OpportunitiesType>(item.Type),
                        AvailableLimit = Convert.ToInt64(item.AvailableLimit ?? 0),
                        Origin = AccountConsts.AccountOrigin.Pluggy,
                        PluggyOpportunityId = item.Id,
                        PluggyItemId = item.ItemId
                    };

                    Opportunity toInsert = ObjectMapper.Map<Opportunity>(opportunityDto);
                    opportunitiesToInsert.Add(toInsert);

                }

                await _opportunityRepository.InsertRangeAsync(opportunitiesToInsert);
            }
        }
    }
}
