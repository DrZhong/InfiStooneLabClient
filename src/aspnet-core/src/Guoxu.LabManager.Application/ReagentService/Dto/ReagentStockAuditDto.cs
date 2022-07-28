using Guoxu.LabManager.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guoxu.LabManager.ReagentService.Dto
{
    [AutoMapper.AutoMap(typeof(ReagentStockAudit))]
    public class ReagentStockAuditDto : CreationAuditedEntityDto
    {
        public int ReagentStockId { get; set; } 
        /// <summary>
        /// 审核类型
        /// </summary>
        public ReagentStockAuditTypeEnum ReagentStockAuditType { get; set; }

        public string AuditUserName { get; set; }
        public long AuditUserId { get; set; }
    }
}
