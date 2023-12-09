namespace Prosperium.Management.ExternalServices.Pluggy
{
    internal class PluggyConsts
    {
        //public const string ClientId = "d91285ed-6beb-41fb-9a28-02c8922f49e0";
        //public const string ClientSecret = "a37d0d45-307b-473b-93ba-5e276719fd67";

        public const string ClientId = "267df39b-90d9-4d83-8c5b-fe64009a988a";
        public const string ClientSecret = "0d6995a7-f5dc-449f-8b9c-137242ca0abb";

        // URLs:
        public const string urlGenerateApiKey = "https://api.pluggy.ai/auth";
        public const string urlCreateConnectToken = "https://api.pluggy.ai/connect_token";
        public const string urlListCategoriesPluggy= "https://api.pluggy.ai/categories";
        public const string urlListTransactionsPluggy= "https://api.pluggy.ai/transactions?accountId={0}";
        public const string urlListAccountsPluggy= "https://api.pluggy.ai/accounts?itemId={0}";
        public const string urlItemPluggy= "https://api.pluggy.ai/items/{0}";
        public const string urlConnectorPluggy= "https://api.pluggy.ai/connectors";
        public const string urlIdentityPluggy= "https://api.pluggy.ai/identity?itemId={0}";

        // URLs for Payments
        public const string urlGetInstitutionsForPayments = "https://api.pluggy.ai/payments/recipients/institutions";
    }
}
