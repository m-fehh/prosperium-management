using Abp.Application.Services;
using Prosperium.Management.OpenAPI.V1.Accounts.Dto;
using Prosperium.Management.OpenAPI.V1.Banks.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prosperium.Management.OpenAPI.V1.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<List<BankDto>> GetAllListBanksAsync();
        Task<List<AccountFinancialDto>> GetAllListAsync();
        Task UpdateBalanceValueAsync(AccountFinancialDto input);
        Task UpdateStatusPluggy(string itemId, string newStatus);
        Task<AccountFinancialDto> GetAccountById(long id);
        Task CreateAsync(AccountFinancialDto input);
        Task<long> CreateAndGetIdAsync(AccountFinancialDto input);
        Task<bool> ValidateAccounts();
    }
}
