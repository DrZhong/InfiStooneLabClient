using System.Collections.Generic;
using Guoxu.LabManager.System.Dto;

namespace Guoxu.LabManager.Sessions.Dto
{
    public class GetCurrentLoginInformationsOutput
    {
        public ApplicationInfoDto Application { get; set; }

        public UserLoginInfoDto User { get; set; }

        public bool CanManageReagent { get; set; }
        public bool CanManageConsum { get; set; }
        public bool CanManageOffice { get; set; }

        public TenantLoginInfoDto Tenant { get; set; }
         
    }
}
