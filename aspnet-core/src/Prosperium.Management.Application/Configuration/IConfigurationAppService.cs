using System.Threading.Tasks;
using Prosperium.Management.Configuration.Dto;

namespace Prosperium.Management.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
