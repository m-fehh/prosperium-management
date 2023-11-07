using Prosperium.Management.Debugging;

namespace Prosperium.Management
{
    public class ManagementConsts
    {
        public const string LocalizationSourceName = "Management";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "ce75fcdade08471dbd5c1ff90a5370f2";
    }
}
