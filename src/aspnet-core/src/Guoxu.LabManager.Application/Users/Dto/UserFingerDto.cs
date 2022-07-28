using Guoxu.LabManager.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guoxu.LabManager.Users.Dto
{
    [AutoMap(typeof(UserFinger))]
    public class UserFingerDto : CreationAuditedEntityDto
    {
        public long UserId { get; set; }

        public string UserName { get; set; }

        public string UserUserName { get; set; }


        #region 指纹数据
        public string Data1 { get; set; }
        public string Data2 { get; set; }
        public string Data3 { get; set; }
        public string Data4 { get; set; }
        public string Data5 { get; set; }
        public string Data6 { get; set; }
        #endregion
    }
}
