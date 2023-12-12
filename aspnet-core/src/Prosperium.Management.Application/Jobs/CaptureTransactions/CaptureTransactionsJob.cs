using Abp.Dependency;
using Prosperium.Management.ExternalServices.Pluggy;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Prosperium.Management.Jobs.CaptureTransactions
{
    public class CaptureTransactionsJob : IJob, ITransientDependency
    {
        private readonly IPluggyAppService _pluggyAppService;

        public CaptureTransactionsJob(IPluggyAppService pluggyAppService)
        {
            _pluggyAppService = pluggyAppService;
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

                    _pluggyAppService.CapturePluggyTransactionsAsync(id, args.DateInitial, args.DateEnd, isCredit);
                }
            }

            return Task.CompletedTask;
        }
    }
}
