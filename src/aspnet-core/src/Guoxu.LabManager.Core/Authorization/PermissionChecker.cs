using Abp.Authorization;
using Guoxu.LabManager.Authorization.Roles;
using Guoxu.LabManager.Authorization.Users;

namespace Guoxu.LabManager.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
