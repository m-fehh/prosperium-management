using Prosperium.Management.Plans;
using Quartz;
using System.Threading.Tasks;

namespace Prosperium.Management.Jobs.CheckPlanIfExpired
{
    public class CheckPlanIfExpiredJob : IJob
    {
        private readonly PlansManager _plansManager;

        public CheckPlanIfExpiredJob(PlansManager plansManager)
        {
            _plansManager = plansManager;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _plansManager.ChangePlanOnExpiration();
        }
    }
}
