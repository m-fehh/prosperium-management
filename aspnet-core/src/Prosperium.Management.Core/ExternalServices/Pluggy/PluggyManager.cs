using Abp.Domain.Services;
using Flurl.Http;
using Prosperium.Management.ExternalServices.Pluggy.Dtos;
using System;
using System.Threading.Tasks;

namespace Prosperium.Management.ExternalServices.Pluggy
{
    public class PluggyManager : IDomainService
    {
        #region Authentication 

        public async Task<string> PluggyGenerateApiKey()
        {
            var result = await PluggyConsts.urlGenerateApiKey
                .PostJsonAsync(new
                {
                    clientId = PluggyConsts.ClientId,
                    clientSecret = PluggyConsts.ClientSecret,
                    nonExpiring = true
                })
                .ReceiveJson<ResultPluggyAuthToken>();

            return result?.apiKey ?? string.Empty;
        }

        public async Task<string> PluggyCreateConnectToken()
        {
            var xApiKey = await PluggyGenerateApiKey();

            var result = await PluggyConsts.urlCreateConnectToken
                .WithHeader("X-API-KEY", xApiKey)
                .PostJsonAsync(new { })
                .ReceiveJson<ResultPluggyAuthToken>();

            return result?.accessToken ?? string.Empty;
        }

        #endregion

        #region Categories Pluggy 

        public async Task<ResultPluggyCategories> PluggyGetCategories()
        {
            var xApiKey = await PluggyGenerateApiKey();

            var result = await PluggyConsts.urlListCategoriesPluggy
                .WithHeader("X-API-KEY", xApiKey)
                .WithHeader("Accept", "application/json")
                .GetJsonAsync<ResultPluggyCategories>();

            return result;
        }

        #endregion

        #region Transactions Pluggy 
        
        public async Task<ResultPluggyTransactions> PluggyGetTransactions(string accountId, DateTime DateInitial, DateTime DateEnd)
        {
            string url = string.Format(PluggyConsts.urlListTransactionsPluggy, accountId, DateInitial.ToString("yyyy-MM-dd"), DateEnd.ToString("yyyy-MM-dd"));
            var xApiKey = await PluggyGenerateApiKey();
            
            var result = await url
                .WithHeader("X-API-KEY", xApiKey)
                .WithHeader("Accept", "application/json")
                .GetJsonAsync<ResultPluggyTransactions>();

            return result;
        }

        #endregion

        #region Connector Pluggy 

        public async Task<ResultPluggyConnector> PluggyGetConnectors()
        {
            var xApiKey = await PluggyGenerateApiKey();
            var result = await PluggyConsts.urlConnectorPluggy
                 .WithHeader("X-API-KEY", xApiKey)
                .WithHeader("Accept", "application/json")
                .GetJsonAsync<ResultPluggyConnector>();

            return result;
        }

        #endregion

        #region Account Pluggy 

        public async Task<ResultPluggyAccounts> PluggyGetAccounts(string itemId)
        {
            string url = string.Format(PluggyConsts.urlListAccountsPluggy, itemId);
            var xApiKey = await PluggyGenerateApiKey();

            var result = await url
                .WithHeader("X-API-KEY", xApiKey)
                .WithHeader("Accept", "application/json")
                .GetJsonAsync<ResultPluggyAccounts>();

            return result;
        }

        #endregion
    }
}
