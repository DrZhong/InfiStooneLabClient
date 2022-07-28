using Guoxu.LabManager.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guoxu.LabManager.ReagentService.Dto
{
    public class TempGerChartDataClass
    {
        public string Time { get; set; }

        public OperateTypeEnum OperateType { get; set; }

        public int Num { get; set; }
    }
}
