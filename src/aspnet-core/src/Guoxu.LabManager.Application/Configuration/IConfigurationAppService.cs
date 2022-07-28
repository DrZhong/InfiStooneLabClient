using System.Threading.Tasks;
using Guoxu.LabManager.Configuration.Dto;

namespace Guoxu.LabManager.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
