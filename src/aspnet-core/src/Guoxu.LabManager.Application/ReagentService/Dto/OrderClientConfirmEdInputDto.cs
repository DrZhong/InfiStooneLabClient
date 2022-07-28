using Guoxu.LabManager.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guoxu.LabManager.ReagentService.Dto
{
    public class OrderConfirmEdInputDto
    {
            
        public int OrderItemId { get; set; }

        public OutOrderMasterItemStatues AuditResult { get; set; }
    }
}
