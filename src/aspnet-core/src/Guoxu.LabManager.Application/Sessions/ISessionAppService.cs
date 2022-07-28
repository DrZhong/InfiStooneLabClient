using System.Threading.Tasks;
using Abp.Application.Services;
using Guoxu.LabManager.Sessions.Dto;

namespace Guoxu.LabManager.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
