using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Guoxu.LabManager.Domains
{
    /// <summary>
    /// 试剂类型
    /// </summary>
    public enum ReagentCatalog
    {
        常规试剂=0,
        标品试剂=1,
        专管试剂=2
    }

    /// <summary>
    /// 试剂状态
    /// </summary>
    public enum ReagentStatus
    {
        液体=0,
        固体,
        气体
    }

    /// <summary>
    /// 安全属性
    /// </summary>
    public enum SafeAttributes
    {
        易制毒=0,      
        易制爆=1,
        剧毒品=2,
        其它=3
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


        [Description("遇水释放易燃气体")]
        遇水释放易燃气体 = 6,
        //[Description("遇水释放易燃气体")]
        //遇水释放易燃气体得性质 = 6,

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
        其他物质 =15
    }

   

    /// <summary>
    /// 试剂基本信息
    /// </summary>
    public class Reagent:FullAuditedEntity,IHasCreateUserName, IMustHaveWarehouseType
    {
        /// <summary>
        /// 客户端入库确认
        /// </summary>
        public bool ClientConfirm { get; set; }
        /// <summary>
        /// 双人双锁
        /// </summary>
        public bool DoubleConfirm { get; set; }

        private ICollection<ReagentLocation> _reagentLocations;

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
        public string CnAliasName{ get; set; }

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

        [MaxLength(512)]
        public string PinYinCode { get; set; }

        /// <summary>
        /// 纯度  字典
        /// </summary>
        public string Purity{ get; set; }
        
        /// <summary>
        /// 容量
        /// </summary>
        public string Capacity { get; set; }


        /// <summary>   
        /// 容量单位 
        /// </summary>
        [MaxLength(512)] 
        public string CapacityUnit { get; set; }
        
        /// <summary>
        /// 存储条件 字典
        /// </summary>
        public string StorageCondition { get; set; }
        /// <summary>
        /// 库存预警
        /// </summary>
        public int InventoryWarning  { get; set; }
            
        #region 供应商
        public int? SupplierCompanyId { get; set; }
        public virtual Company SupplierCompany { get; set; }
        public string SupplierCompanyName { get; set; }
        #endregion


        #region 生产商
        public int? ProductionCompanyId { get; set; }
        public virtual Company ProductionCompany { get; set; }
        public string ProductionCompanyName { get; set; } 
        #endregion

        /// <summary>
        /// 存储位置
        /// </summary>
        public virtual ICollection<ReagentLocation> ReagentLocations
        {
            get => _reagentLocations ??= new List<ReagentLocation>();
            set => _reagentLocations = value;
        }


        [MaxLength(512)]
        public string CreateUserName { get; set; }

        /// <summary>
        /// 试剂仓库专属
        /// </summary>
        public WarehouseType WarehouseType { get; set; }

        /// <summary>
        /// 参考价格
        /// </summary>
        public decimal Price { get; set; }
    }


    /// <summary>
    /// 试剂存放位置
    /// </summary>
    public class ReagentLocation:Entity
    {
        /// <summary>
        /// 所属试剂
        /// </summary>
        public int ReagentId { get; set; }
        public virtual Reagent Reagent { get; set; }

        /// <summary>
        /// 所属位置
        /// </summary>
        public int LocationId { get; set; }
        public virtual Location Location { get; set; } 
    }
}
