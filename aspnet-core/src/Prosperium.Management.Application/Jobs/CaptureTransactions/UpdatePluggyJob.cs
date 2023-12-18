using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.Extensions.Logging;
using Prosperium.Management.ExternalServices.Pluggy;
using Prosperium.Management.MultiTenancy;
using Prosperium.Management.OpenAPI.V1.Accounts;
using Prosperium.Management.OpenAPI.V1.Accounts.Dto;
using Prosperium.Management.OpenAPI.V1.Transactions;
using Quartz;
using Quartz.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Prosperium.Management.Jobs.CaptureTransactions
{
    public class UpdatePluggyJob : IJob, ITransientDependency
    {
        private readonly IPluggyAppService _pluggyAppService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IAccountAppService _accountAppService;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly ILogger<UpdatePluggyJob> _logger;

        public UpdatePluggyJob(IPluggyAppService pluggyAppService, IUnitOfWorkManager unitOfWorkManager, IAccountAppService accountAppService, IRepository<Tenant> tenantRepository, ILogger<UpdatePluggyJob> logger)
        {
            _pluggyAppService = pluggyAppService;
            _unitOfWorkManager = unitOfWorkManager;
            _accountAppService = accountAppService;
            _tenantRepository = tenantRepository;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                foreach (var tenant in GetAllTenants())
                {
                    var accounts = await GetAllAccounts(tenant.Id);

                    foreach (var account in accounts)
                    {
                        try
                        {
                            // Atualiza o connector Pluggy
                            await _pluggyAppService.PluggyUpdateItemAsync(account.PluggyItemId);
                            Thread.Sleep(8000);

                            await _pluggyAppService.UpdateAllDataPluggy(account.PluggyItemId, account.Id);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.Message, ex);
                            continue;
                        }
                    }
                }

                await unitOfWork.CompleteAsync();
            }
        }

        #region PRIVATE METHODS  

        private List<Tenant> GetAllTenants()
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var tenants = (_tenantRepository.GetAllList()).Where(x => x.TenancyName != "Default").ToList();
                unitOfWork.Complete();

                return tenants;
            }
        }

        private async Task<List<AccountFinancialDto>> GetAllAccounts(int tenantId)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var accounts = (await _accountAppService.GetAllListAsync()).Where(x => x.Bank.Name.ToUpper() != "BANCO INTER S.A.");
                unitOfWork.Complete();

                return accounts.Where(x => x.TenantId == tenantId && x.Origin == AccountConsts.AccountOrigin.Pluggy).ToList();
            }
        }
        #endregion
    }
}
