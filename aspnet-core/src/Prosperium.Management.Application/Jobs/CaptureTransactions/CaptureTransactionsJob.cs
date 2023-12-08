using Abp.BackgroundJobs;
using Abp.Dependency;
using Prosperium.Management.OpenAPI.V1.Transactions;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Prosperium.Management.Jobs.CaptureTransactions
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
                    // Split na string usando "-"
                    var Type = id.Split('-')[0];

                    bool isCredit = Type.Length > 0 && Type.Equals("CREDIT", StringComparison.OrdinalIgnoreCase);

                    _transactionAppService.CapturePluggyTransactionsAsync(id, args.DateInitial, args.DateEnd, isCredit);
                }
            }

            return Task.CompletedTask;
        }
    }
}
