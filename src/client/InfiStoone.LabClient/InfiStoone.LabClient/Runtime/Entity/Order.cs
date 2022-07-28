using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiStoone.LabClient.Runtime.Entity
{
    public enum OutOrderStatusEnum
    {
        待出库,
        出库完毕,
        取消
    }
    /// <summary>
    /// 出库单审核 状态
    /// </summary>
    public enum OutOrderMasterItemStatues
    {
        待审核 = 0,
        审核通过 = 1,
        审核不通过 = 2
    }
    public enum OutOrderTypeEnum
    {
        专管试剂 = 1,
        普通试剂
    }

    public class OutOrder:EntityBase
    {
        private ICollection<OutOrderMasterItem> outOrderMasterItems;

        public OutOrderStatusEnum OutOrderStatus { get; set; }

        public string OutOrderStatusString  => OutOrderStatus.ToString();

        /// <summary>
        /// 所属仓库
        /// </summary>
        public int WarehouseId { get; set; }    

        public string WarehouseName { get; set; }

        public OutOrderTypeEnum OutOrderType { get; set; }
        public bool IsMaster => OutOrderType == OutOrderTypeEnum.专管试剂;

        /// <summary>   
        /// 申请人 
        /// </summary>
        public long? ApplyUserId { get; set; }
        public string ApplyUserName { get; set; }
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 订单项
        /// </summary>
        public virtual ICollection<OutOrderMasterItem> OutOrderMasterItems
        {
            get => outOrderMasterItems ?? (outOrderMasterItems = new List<OutOrderMasterItem>());
            set => outOrderMasterItems = value;
        }
    }

    /// <summary>   
    /// 专管出库单详情
    /// </summary>
    public class OutOrderMasterItem : EntityBase, INotifyPropertyChanged
    {
        public decimal Price { get; set; }
        /// <summary>
        /// 二维码或条形码
        /// </summary> 
        public string ReagentStockBarCode { get; set; }

        /// <summary>
        /// 批次
        /// </summary> 
        public string ReagentStockBatchNo { get; set; }

        /// <summary>
        /// 容量  规格 冗余自试剂基本信息
        /// </summary>
        public string ReagentStockCapacity { get; set; }


        /// <summary>   
        /// 容量单位   规格单位 冗余自试剂基本信息 
        public string ReagentStockCapacityUnit { get; set; }


        /// <summary>
        /// 安全属性
        /// 若该项选择，该试剂为专管试剂，也可不选，若该项不选择试剂为普通试剂
        /// </summary>
        public SafeAttributes? ReagentStockSafeAttribute { get; set; }

        public int OutOrderId { get; set; } 

        /// <summary>
        /// 客户端入库确认
        /// </summary>
        public bool ClientConfirm { get; set; }

        public string ClientConfirmString => ClientConfirm ? ClientConfirmed.ToString() : "不用审核";
        /// <summary>
        /// 双人双锁
        /// </summary>
        public bool DoubleConfirm { get; set; }
        public string DoubleConfirmString => DoubleConfirm ? DoubleConfirmed.ToString() : "不用审核";

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

        /// <summary>
        /// 出库数量
        /// </summary>
        public int StockoutAccount { get; set; }    
            

        public int ScanedAccount { get; set; }

        public bool Scaned { get; set; }


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

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        /// <summary>
        /// 针对那个库存
        /// </summary>
        //public int? NormalReagentStockId { get; set; }
        //public virtual NormalReagentStock NormalReagentStock { get; set; }
    }

}
