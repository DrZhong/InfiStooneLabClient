using Guoxu.LabManager.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guoxu.LabManager.Users.Dto
{
    [AutoMap(typeof(UserFinger))]
    public class UserFingerListDto : CreationAuditedEntityDto
    {
        public long UserId { get; set; }

        public string UserName { get; set; }

        public string UserUserName { get; set; } 
    }
}
