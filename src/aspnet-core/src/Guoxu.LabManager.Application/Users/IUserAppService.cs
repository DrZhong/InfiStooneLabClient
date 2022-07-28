using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Guoxu.LabManager.Roles.Dto;
using Guoxu.LabManager.Users.Dto;

namespace Guoxu.LabManager.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        Task DeActivate(EntityDto<long> user);
        Task Activate(EntityDto<long> user);
        Task<ListResultDto<RoleDto>> GetRoles();
        Task ChangeLanguage(ChangeUserLanguageDto input);

        Task<bool> ChangePassword(ChangePasswordDto input);
    }
}
