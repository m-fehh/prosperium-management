using Abp.Dependency;
using Abp.Domain.Uow;
using Prosperium.Management.ExternalServices.Pluggy;
using Prosperium.Management.ExternalServices.Pluggy.Dtos;
using Quartz;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Prosperium.Management.Jobs.CaptureTransactions
{
    public class CaptureTransactionsJob : IJob, ITransientDependency
    {
        private readonly IPluggyAppService _pluggyAppService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public CaptureTransactionsJob(IPluggyAppService pluggyAppService, IUnitOfWorkManager unitOfWorkManager)
        {
            _pluggyAppService = pluggyAppService;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var args = context.JobDetail.JobDataMap.Get("CaptureTransactionsArgs") as CaptureTransactionsArgs;
                if (args != null && args.IdsAccountOrCard.Count > 0)
                {
                    foreach (var id in args.IdsAccountOrCard)
                    {
                        var pluggyTypeAccountId = await SplitForParameterAsync(id);

                        bool isCredit = pluggyTypeAccountId.TypeAccount.Equals("CREDIT", StringComparison.OrdinalIgnoreCase);

                        // Atualiza o connector Pluggy
                        var accountUpdate = await _pluggyAppService.PluggyUpdateItemAsync(pluggyTypeAccountId.PluggyAccountId);

                        if (accountUpdate.Connector.Credentials.Count > 0)
                        {
                            if(accountUpdate.Status.ToUpper() == "UPDATING")
                            {
                                Thread.Sleep(8000);
                            }

                            // Captura as transações
                            await _pluggyAppService.CapturePluggyTransactionsAsync(pluggyTypeAccountId.PluggyAccountId, args.DateInitial, args.DateEnd, isCredit, args.TenantId);

                            // Atualiza o balance da conta e/ou o limite do cartão
                            await ((isCredit) ? _pluggyAppService.UpdateLimitCard(pluggyTypeAccountId.PluggyAccountId) : _pluggyAppService.UpdateBalanceAccount(pluggyTypeAccountId.PluggyAccountId));
                        }
                    }

                    await unitOfWork.CompleteAsync();
                }
            }
        }

        private async Task<(string TypeAccount, string PluggyAccountId)> SplitForParameterAsync(string input)
        {
            var splitData = input.Split('@');

            var typeAccount = splitData[0];
            var pluggyAccountId = splitData[1];

            return (typeAccount, pluggyAccountId);
        }
    }
}
