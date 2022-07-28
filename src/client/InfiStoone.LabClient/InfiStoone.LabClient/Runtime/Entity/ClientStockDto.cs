using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiStoone.LabClient.Runtime.Entity
{
    /// <summary>
    /// 试剂类型
    /// </summary>
    public enum ReagentCatalog
    {
        常规试剂 = 0,
        标品试剂 = 1
    }

    /// <summary>
    /// 试剂状态
    /// </summary>
    public enum ReagentStatus
    {
        液体 = 0,
        固体,
        气体
    }

    /// <summary>
    /// 安全属性
    /// </summary>
    public enum SafeAttributes
    {
        易制毒 = 0,
        易制爆 = 1
    }

    public enum StorageAttrEnum
    {
        [Description("爆炸品")]
        爆炸品 = 1,

        [Description("气体-易燃")]
        气体_易燃 = 2,

        [Description("气体惰性")]
        气体惰性 = 3,

        [Description("易燃固体")]
        易燃固体 = 4,

        [Description("易燃液体")]
        易燃液体 = 5,

        [Description("遇水释放易燃气体得性质")]
        遇水释放易燃气体得性质 = 6,

        [Description("易于自燃的物质-自燃")]
        易于自燃的物质_自燃 = 7,

        [Description("易于自燃的物质-发火")]
        易于自燃的物质_发火 = 8,

        [Description("氧化性物质")]
        氧化性物质 = 9,

        [Description("有机过氧化物")]
        有机过氧化物 = 10,

        [Description("毒性物质-剧毒")]
        毒性物质_剧毒 = 11,

        [Description("毒性物质-其它")]
        毒性物质_其它 = 12,

        [Description("腐蚀性物质-酸性")]
        腐蚀性物质_酸性 = 13,

        [Description("腐蚀性物质碱性及其它")]
        腐蚀性物质碱性及其它 = 14,

        [Description("其他物质")]
        其他物质 = 15
    }
    public class ClientStockDto:EntityBase
    {
        /// <summary>
        /// 是否专管库存
        /// </summary>
        public bool IsMaster { get; set; } 
        public string BarCode { get; set; }
        /// <summary>
        /// 存储位置
        /// </summary>
        public string LocationName { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 最小价格
        /// </summary>
        public decimal MinPrice { get; set; }
        /// <summary>
        /// 最大价格
        /// </summary>
        public decimal MaxPrice { get; set; }

        public string PriceRang => $"{this.MinPrice:N} ~ {this.MaxPrice:N}";

        public decimal Price { get; set; }

        public decimal Weight { get; set; }
        /// <summary>
        /// 准备出库数量 
        /// </summary>
        public int PrepareToStockOutNum { get; set; } = 1;

        /// <summary>
        /// 试剂编号
        /// </summary>
        [Required]
        [MaxLength(512)]
        public string No { get; set; }

        /// <summary>
        /// 试剂类型
        /// </summary>
        public ReagentCatalog ReagentCatalog { get; set; }

        /// <summary>
        /// cas号
        /// </summary>
        [MaxLength(512)]
        public string CasNo { get; set; }

        /// <summary>
        /// 试剂状态
        /// </summary>
        public ReagentStatus ReagentStatus { get; set; }

        [Required]
        [MaxLength(512)]
        public string CnName { get; set; }
        public string CnAliasName { get; set; }

        [MaxLength(1024)]
        public string EnName { get; set; }
        /// <summary>
        /// 安全属性
        /// 若该项选择，该试剂为专管试剂，也可不选，若该项不选择试剂为普通试剂
        /// </summary>
        public SafeAttributes? SafeAttribute { get; set; }

        /// <summary>
        /// 存储属性 
        /// </summary>
        public StorageAttrEnum StorageAttr { get; set; }
         
        public string PinYinCode { get; set; }

        /// <summary>
        /// 纯度  字典
        /// </summary>
        public string Purity { get; set; }

        /// <summary>
        /// 容量
        /// </summary>
        public string Capacity { get; set; }
        /// <summary>
        /// 容量单位 字典
        /// </summary> 
        public string CapacityUnit { get; set; }

        /// <summary>
        /// 存储条件 字典
        /// </summary>
        public string StorageCondition { get; set; }
        /// <summary>
        /// 库存预警
        /// </summary>
        public int InventoryWarning { get; set; }

        #region 供应商
        public int? SupplierCompanyId { get; set; }
        public string SupplierCompanyName { get; set; }
        #endregion


        #region 生产商
        public int? ProductionCompanyId { get; set; }
        public string ProductionCompanyName { get; set; }
        #endregion
 
          
        public string CreateUserName { get; set; }
    }
}
