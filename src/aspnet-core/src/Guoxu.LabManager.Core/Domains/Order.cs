using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using Abp.Domain.Entities.Auditing;
using Guoxu.LabManager.Authorization.Users;
using System.ComponentModel.DataAnnotations;

namespace Guoxu.LabManager.Domains
{
    public enum OutOrderStatusEnum
    {
        待出库,
        出库完毕,
        取消
    }

    public enum OutOrderTypeEnum
    {
        专管试剂=1,
        普通试剂
    }

    /// <summary>
    /// 出库单
    /// </summary>
    public class OutOrder : CreationAuditedEntity
    {
 

        private ICollection<OutOrderMasterItem> outOrderMasterItems;

        public OutOrderStatusEnum OutOrderStatus { get; set; }

        /// <summary>
        /// 所属仓库
        /// </summary>
        public int WarehouseId { get; set; }

        public string WarehouseName { get; set; }

        public OutOrderTypeEnum OutOrderType { get; set; }

        /// <summary>   
        /// 申请人
        /// </summary>
        public long? ApplyUserId { get; set; }

        /// <summary>
        /// 冗余
        /// </summary>
        public string ApplyUserName { get; set; }
        public virtual User ApplyUser { get; set; }

        /// <summary>
        /// 订单项
        /// </summary>
        public virtual ICollection<OutOrderMasterItem> OutOrderMasterItems {
            get => outOrderMasterItems??(outOrderMasterItems=new List<OutOrderMasterItem>()); 
            set => outOrderMasterItems = value; 
        }
    }

    /// <summary>
    /// 出库单审核 状态
    /// </summary>
    public enum OutOrderMasterItemStatues
    {
        待审核=0,
        审核通过=1,
        审核不通过=2
    }

    /// <summary>   
    /// 专管出库单详情
    /// </summary>
    public class OutOrderMasterItem : Entity
    {
        public decimal Price { get; set; }
        public int OutOrderId { get; set; }
        public virtual OutOrder OutOrder { get; set; }

        /// <summary>
        /// 客户端入库确认
        /// </summary>
        public bool ClientConfirm { get; set; }
        /// <summary>
        /// 双人双锁
        /// </summary>
        public bool DoubleConfirm { get; set; }

        /// <summary>
        /// 客户端入库确认 审核是否通过
        /// </summary>
        public OutOrderMasterItemStatues ClientConfirmed { get; set; }
        /// <summary>
        /// 双人双锁   审核是否通过
        /// </summary>
        public OutOrderMasterItemStatues DoubleConfirmed { get; set; }

        /// <summary>
        /// 针对那个库存
        /// </summary>
        public int? ReagentStockId { get; set; }     
        public virtual ReagentStock ReagentStock { get; set; }

        /// <summary>
        /// 出库数量
        /// </summary>
        public int StockoutAccount { get; set; }

        /// <summary>
        /// 出库时间
        /// </summary>
        public DateTime? StockOutTime   { get; set; }

        #region 如果时普通试剂的话  需要指定从哪个库位出什么试剂
        public int? ReagentId { get; set; }
        /// <summary>
        /// 试剂编号
        /// </summary> 
        public string ReagentNo { get; set; }
 
        public string ReagentCnName { get; set; }
        public string ReagentCnAliasName { get; set; }
 
        public string ReagentEnName { get; set; }
        /// <summary>
        /// 试剂类型
        /// </summary>
        public ReagentCatalog ReagentReagentCatalog { get; set; }

        /// <summary>
        /// cas号
        /// </summary>
        [MaxLength(512)]
        public string ReagentCasNo { get; set; }

        /// <summary>
        /// 纯度
        /// </summary>
        public string ReagentPurity { get; set; }
        

        /// <summary>
        /// 所属位置
        /// </summary>
        public int? LocationId { get; set; }
        /// <summary>
        /// 位置名称
        /// </summary>
        public string LocationName { get; set; }
        #endregion
        /// <summary>
        /// 针对那个库存
        /// </summary>
        //public int? NormalReagentStockId { get; set; }
        //public virtual NormalReagentStock NormalReagentStock { get; set; }
    }


    /// <summary> 
    /// 出库单 入库确认、出入库双人双审核
    /// </summary>
    public class OutOrderMasterItemAudit : CreationAuditedEntity
    {
        public int OutOrderMasterItemId { get; set; }
        public virtual OutOrderMasterItem OutOrderMasterItem { get; set; }
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
        public virtual User AuditUser { get; set; }
    }
}
