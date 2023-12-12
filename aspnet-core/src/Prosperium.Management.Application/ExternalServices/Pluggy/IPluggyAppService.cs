using Abp.Application.Services;
using System;
using System.Threading.Tasks;

namespace Prosperium.Management.ExternalServices.Pluggy
{
    public interface IPluggyAppService : IApplicationService
    {
        Task PluggyCreateAsync(string itemId);
        Task CapturePluggyTransactionsAsync(string accountId, DateTime? dateInitial, DateTime? dateEnd, bool isCreditCard);
    }
}
