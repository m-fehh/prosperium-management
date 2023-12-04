using Abp.Application.Services;
using Prosperium.Management.OpenAPI.V1.Accounts.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prosperium.Management.OpenAPI.V1.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<List<BankDto>> GetAllListBanksAsync();
        Task<List<AccountFinancialDto>> GetAllListAsync();
        Task<AccountFinancialDto> GetAccountById(long id);
        Task PluggyCreateAccount(string itemId);
    }
}
