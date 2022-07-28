using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;

namespace Guoxu.LabManager.Authorization.Users
{
    /// <summary>
    /// 用户指纹
    /// </summary>
    public class UserFinger:CreationAuditedEntity
    {
        public long UserId { get; set; }

        public virtual User User { get; set; }


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
