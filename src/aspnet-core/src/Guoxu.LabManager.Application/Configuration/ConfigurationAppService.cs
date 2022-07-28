using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Guoxu.LabManager.Configuration.Dto;

namespace Guoxu.LabManager.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : LabManagerAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
