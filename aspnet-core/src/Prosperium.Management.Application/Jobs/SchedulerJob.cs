using Abp.Domain.Uow;
using Quartz;
using System;
using System.Threading.Tasks;
using Prosperium.Management.Jobs.CaptureTransactions;
using Abp.Quartz;
using Prosperium.Management.Jobs.CheckPlanIfExpired;

namespace Prosperium.Management.Jobs
{
    public class SchedulerJob : ManagementAppServiceBase
    {
        private readonly IQuartzScheduleJobManager _jobManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public SchedulerJob(IQuartzScheduleJobManager jobManager, IUnitOfWorkManager unitOfWorkManager)
        {
            _jobManager = jobManager;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task StartScheduler()
        {
            await StartTransactionCaptureJob();
            await StartCheckPlanIfExpired();
        }

        private async Task StartTransactionCaptureJob()
        {
            await _jobManager.ScheduleAsync<UpdatePluggyJob>
            (
                async job =>
                {
                    job.WithIdentity($"{nameof(UpdatePluggyJob)}_{DateTime.Now.ToString("dd/MM/yyyy")}", "TransactionJobs")
                    .WithDescription($"Job to capture and process transactions.");

                },
                trigger =>
                {
                    trigger.WithCronSchedule($"0 25 22 * * ?");
                }
            );
        }

        private async Task StartCheckPlanIfExpired()
        {
            await _jobManager.ScheduleAsync<CheckPlanIfExpiredJob>
            (
                async job =>
                {
                    job.WithIdentity("CheckPlanIfExpired", "CheckPlanIfExpiredJob")
                        .WithDescription("Job to validate and change expired plans");
                },
                trigger =>
                {
                    trigger.WithCronSchedule($"0 00 19 * * ?");
                    //trigger.WithCronSchedule("0 0 8 * * ?"); // Executar todos os dias às 8h

                }
            );
        }
    }
}
