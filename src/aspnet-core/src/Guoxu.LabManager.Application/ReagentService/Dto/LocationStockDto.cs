using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guoxu.LabManager.ReagentService.Dto
{
    public class LocationStockDto
    {
        /// <summary>   
        /// 如果非0 则限制此货位的最大存货量
        /// </summary>
        public int CountLimit { get; set; }

        /// <summary>
        /// 在库数量
        /// </summary>
        public int ExitStockCount { get; set; }

        /// <summary>
        /// 离库数量
        /// </summary>
        public int OutStockCount { get; set; }
    }
}
