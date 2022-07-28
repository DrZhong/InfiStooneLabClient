using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guoxu.LabManager.Commons.Dto
{
    public class StockOutInputDto
    { 
        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }
            
        public int WarehouseId { get; set; }

        public decimal Weight { get; set; }

        /// <summary>
        /// 出库数量
        /// </summary>
        public int Num { get; set; }
    }
}
