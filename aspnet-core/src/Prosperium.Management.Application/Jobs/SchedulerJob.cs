using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Prosperium.Management.ExternalServices.Pluggy;
using Prosperium.Management.MultiTenancy;
using Prosperium.Management.OpenAPI.V1.Accounts;
using Prosperium.Management.OpenAPI.V1.Transactions;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Prosperium.Management.Jobs.CaptureTransactions;
using Abp.Quartz;

namespace Prosperium.Management.Jobs
{
    public class SchedulerJob : ManagementAppServiceBase
    {
        private readonly IQuartzScheduleJobManager _jobManager;
        private readonly ITransactionAppService _transactionAppService;
        private readonly IAccountAppService _accountAppService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly PluggyManager _pluggyManager;
        private readonly IRepository<Tenant> _tenantRepository;

        private readonly IHostingEnvironment _hostingEnvironment;

        public SchedulerJob(
            IQuartzScheduleJobManager jobManager,
            ITransactionAppService transactionAppService,
            IAccountAppService accountAppService,
            IUnitOfWorkManager unitOfWorkManager,
            PluggyManager pluggyManager,
            IRepository<Tenant> tenantRepository,
            IHostingEnvironment hostingEnvironment)
        {
            _jobManager = jobManager;
            _transactionAppService = transactionAppService;
            _accountAppService = accountAppService;
            _unitOfWorkManager = unitOfWorkManager;
            _pluggyManager = pluggyManager;
            _tenantRepository = tenantRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task StartScheduler()
        {
            await StartTransactionCaptureJob();
        }

        private async Task StartTransactionCaptureJob()
        {
            var tenants = GetAllTenants();

            foreach (var tenant in tenants)
            {
                var captureTransactionsArgs = await CreateCaptureTransactionsArgs(tenant);

                await _jobManager.ScheduleAsync<CaptureTransactionsJob>(
                    async job =>
                    {
                        job.WithIdentity($"{nameof(CaptureTransactionsJob)}_{Guid.NewGuid()}_Tenant_{tenant.Id}", "TransactionJobs")
                            .WithDescription($"Job to capture and process transactions for Tenant {tenant.Id}.");

                        job.UsingJobData(new JobDataMap { { "CaptureTransactionsArgs", captureTransactionsArgs } });
                    },
                    trigger =>
                    {
                        trigger.WithCronSchedule("0 0 20 * * ?"); // Executa todos os dias às 20h
                    });
            }
        }

        #region PRIVATE METHODS  

        private async Task<CaptureTransactionsArgs> CreateCaptureTransactionsArgs(Tenant tenant)
        {
            DateTime dateInitial = DateTime.Now.Date.AddDays(-1);
            DateTime dateEnd = dateInitial.AddDays(2).AddTicks(-1);

            var idsAccountOrCard = await GetItemIdPluggyByTenant(tenant.Id);

            return new CaptureTransactionsArgs
            {
                DateInitial = dateInitial,
                DateEnd = dateEnd,
                IdsAccountOrCard = idsAccountOrCard,
                TenantId = tenant.Id
            };
        }


        private async Task<List<string>> GetItemIdPluggyByTenant(int tenantId)
        {
            List<string> idsForFindTransactions = new List<string>();


            var allItemsIdsForPluggy = await GetAllAccounts(tenantId);


            foreach (var itemId in allItemsIdsForPluggy)
            {
                var itemsPluggy = await _pluggyManager.PluggyGetAccountAsync(itemId);
                if (itemsPluggy.Total > 0)
                {
                    idsForFindTransactions.AddRange(itemsPluggy.Results.Select(item => $"{item.Type}-{item.Id}"));
                }
            }


            return idsForFindTransactions;
        }

        private List<Tenant> GetAllTenants()
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var tenants = _tenantRepository.GetAllList();
                unitOfWork.Complete();

                return tenants;
            }
        }

        private async Task<List<string>> GetAllAccounts(int tenantId)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var accounts = await _accountAppService.GetAllListAsync();
                unitOfWork.Complete();

                return accounts.Where(x => x.TenantId == tenantId && x.Origin == AccountConsts.AccountOrigin.Pluggy).Select(x => x.PluggyItemId).ToList();
            }
        }
        #endregion
    }
}
