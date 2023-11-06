using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Prosperium.Management.Configuration.Dto;

namespace Prosperium.Management.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : ManagementAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
