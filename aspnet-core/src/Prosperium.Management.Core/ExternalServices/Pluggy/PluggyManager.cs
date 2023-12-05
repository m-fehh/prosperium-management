using Abp.Domain.Services;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json.Linq;
using Prosperium.Management.ExternalServices.Pluggy.Dtos;
using System;
using System.Globalization;
using System.Linq;
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

            foreach (var item in result.Results)
            {
                if (item.ParentDescription != null)
                {
                    item.ParentDescriptionTranslated = await TranslateTextAsync(item.ParentDescription, "en", "pt");
                }
            }

            return result;
        }

        private async Task<string> TranslateTextAsync(string text, string fromLanguage, string toLanguage)
        {
            string endpoint = $"https://translate.googleapis.com/translate_a/single";

            var result = await endpoint
                .SetQueryParam("client", "gtx")
                .SetQueryParam("sl", fromLanguage)
                .SetQueryParam("tl", toLanguage)
                .SetQueryParam("dt", "t")
                .SetQueryParam("q", text)
                .GetJsonAsync<JArray>();

            string translation = result?[0]?.FirstOrDefault()?.FirstOrDefault()?.ToString();

            return translation;
        }

        #endregion

        #region Transactions Pluggy 

        public async Task<ResultPluggyTransactions> PluggyGetTransactions(string accountId, DateTime? dateInitial = null, DateTime? dateEnd = null)
        {
            string url = string.Format(PluggyConsts.urlListTransactionsPluggy, accountId);

            if (dateInitial.HasValue && dateEnd.HasValue)
            {
                url += $"&from={dateInitial.Value.ToString("yyyy-MM-dd")}&to={dateEnd.Value.ToString("yyyy-MM-dd")}";
            }

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

        #region Items Pluggy 

        public async Task<ResultPluggyItem> PluggyGetItemId(string itemId)
        {
            string url = string.Format(PluggyConsts.urlItemPluggy, itemId);
            var xApiKey = await PluggyGenerateApiKey();

            var result = await url
                .WithHeader("X-API-KEY", xApiKey)
                .WithHeader("Accept", "application/json")
                .GetJsonAsync<ResultPluggyItem>();

            return result;
        }

        public async Task PluggyItemDeleteAsync(string itemId)
        {
            string url = string.Format(PluggyConsts.urlItemPluggy, itemId);
            var xApiKey = await PluggyGenerateApiKey();

             await url.WithHeader("X-API-KEY", xApiKey).WithHeader("Accept", "application/json").DeleteAsync();
        }

        #endregion

        #region Account Pluggy 

        public async Task<ResultPluggyAccounts> PluggyGetAccount(string itemId)
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
