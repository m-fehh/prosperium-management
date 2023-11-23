using Microsoft.Extensions.Configuration;

namespace Prosperium.Management.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
