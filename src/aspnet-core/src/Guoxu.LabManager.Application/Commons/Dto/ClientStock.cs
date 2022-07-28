using Abp.AutoMapper;
using Guoxu.LabManager.BaseInfo.Dto;
using Guoxu.LabManager.Domains;

namespace Guoxu.LabManager.Commons.Dto
{
    /// <summary>
    /// 客户端库存
    /// </summary>
    [AutoMapFrom(typeof(NormalReagentStock))]
    public class ClientStockDto: ReagentDto
    {
        /// <summary>
        /// 是否专管库存
        /// </summary>
        public bool IsMaster { get; set; }

        /// <summary>
        /// 存储位置
        /// </summary>
        public string LocationName { get; set; }
        /// <summary>
        /// 存储位置
        /// </summary>
        public int? LocationId { get; set; }

        public string WarehouseName { get; set; }

        /// <summary>
        /// 标识位
        /// 标识此 库存正在被哪个出库单锁住
        /// </summary>
        public int? LockedOutOrderId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        public int  Num { get; set; }

        /// <summary>
        /// 总总量
        /// </summary>
        public decimal TotalWeight { get; set; }

        /// <summary>
        /// 最小价格
        /// </summary>
        public decimal MinPrice { get; set; }
        /// <summary>
        /// 最大价格
        /// </summary>
        public decimal MaxPrice { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }


        public decimal Weight { get; set; }
    }
}
