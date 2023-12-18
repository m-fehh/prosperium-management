using Abp.Application.Services;
using Prosperium.Management.ExternalServices.Pluggy.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prosperium.Management.ExternalServices.Pluggy
{
    public interface IPluggyAppService : IApplicationService
    {
        Task<List<string>> PluggyCreateAsync(string itemId);
        Task CaptureOpportunityAsync(string itemId);
        Task CapturePluggyTransactionsAsync(string pluggyAccountOrCardId, string pluggyItemId, DateTime? dateInitial, DateTime? dateEnd, bool isCreditCard, int tenantId);
        Task UpdateBalanceAccount(string itemId);
        Task UpdateLimitCard(string itemId);
        Task<ResultPluggyItem> PluggyUpdateItemAsync(string itemId);
        Task UpdateAllDataPluggy(string itemId, long accountId);
    }
}
