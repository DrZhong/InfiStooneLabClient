using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Guoxu.LabManager.Controllers
{
    public abstract class LabManagerControllerBase: AbpController
    {
        protected LabManagerControllerBase()
        {
            LocalizationSourceName = LabManagerConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
