namespace Prosperium.Management.ExternalServices.Pluggy
{
    internal class PluggyConsts
    {
        public const string ClientId = "d91285ed-6beb-41fb-9a28-02c8922f49e0";
        public const string ClientSecret = "a37d0d45-307b-473b-93ba-5e276719fd67";

        // URLs:
        public const string urlGenerateApiKey = "https://api.pluggy.ai/auth";
        public const string urlCreateConnectToken = "https://api.pluggy.ai/connect_token";
        public const string urlListCategoriesPluggy= "https://api.pluggy.ai/categories";
        public const string urlListTransactionsPluggy= "https://api.pluggy.ai/transactions?accountId={0}&from={1}&to={2}";
    }
}
