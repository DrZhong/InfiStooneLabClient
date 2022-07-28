using Guoxu.LabManager.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guoxu.LabManager.ReagentService.Dto
{
    public class GetMasterReagentStockInputDto: PagedResultRequestFilterDto
    {
        public string No { get; set; }
        public string CasNo { get; set; }

        public string Purity { get; set; }
        /// <summary>
        /// 存储条件 字典
        /// </summary>
        public string StorageCondition { get; set; }

        public int? WarehouseId { get; set; }

        /// <summary>
        /// 1 濒临过期
        /// 2 已过期
        /// 3 库存紧张
        /// </summary>
        public int? StockStatus { get; set; }

        public bool GroupByLocationId { get; set; }

        /// <summary>
        /// 库存应该大于0
        /// </summary>
        public bool StockShouldMoreZero { get; set; }
    }

    public class GetMasterReagentStockDetailByNoInput: PagedInputDto
    {

        [Required]
        public string RegentNo { get; set; }

        /// <summary>
        /// 1 濒临过期
        /// 2 已过期
        /// 3 库存紧张
        /// </summary>
        public int? StockStatus { get; set; } 

    }
}
