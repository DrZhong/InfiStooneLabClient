using System.Threading.Tasks;
using Abp.Application.Services;
using Guoxu.LabManager.Authorization.Accounts.Dto;

namespace Guoxu.LabManager.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
