using Guoxu.LabManager.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guoxu.LabManager.ReagentService.Dto
{
    [AutoMapFrom(typeof(OutOrderMasterItemAudit))]
    public class OutOrderMasterItemAuditDto : CreationAuditedEntityDto
    {
        public int OutOrderMasterItemId { get; set; } 
        /// <summary>
        /// 审核类型
        /// </summary>
        public ReagentStockAuditTypeEnum ReagentStockAuditType { get; set; }

        /// <summary>
        /// 审核结果
        /// 通过 或者 不通过
        /// </summary>
        public OutOrderMasterItemStatues AuditResult { get; set; }

        public string AuditUserName { get; set; }
        public long AuditUserId { get; set; } 
    }
}
