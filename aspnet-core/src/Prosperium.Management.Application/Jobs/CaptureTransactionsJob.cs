using Abp.Dependency;
using Abp.Quartz;
using Quartz;
using Prosperium.Management.OpenAPI.V1.Transactions;
using System;
using System.Threading.Tasks;

namespace Prosperium.Management.Jobs
{
    public class CaptureTransactionsJob : IJob, ITransientDependency
    {
        private readonly ITransactionAppService _transactionAppService;

        public CaptureTransactionsJob(ITransactionAppService transactionAppService)
        {
            _transactionAppService = transactionAppService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var args = context.JobDetail.JobDataMap.Get("CaptureTransactionsArgs") as CaptureTransactionsArgs;

            if (args != null && args.IdsAccountOrCard.Count > 0)
            {
                foreach (var id in args.IdsAccountOrCard)
                {
                    _transactionAppService.CapturePluggyTransactionsAsync(id, args.DateInitial, args.DateEnd); 
                }
            }

            return Task.CompletedTask;
        }
    }
}
