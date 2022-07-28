using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiStoone.LabClient.Runtime.Entity
{
    public class ReagentStockDto : CreationAuditedEntityDto
    {
        private bool canStaockIn;
        /// <summary>
        /// 参考价格
        /// </summary>
        public decimal Price { get; set; }  
        public decimal Weight { get; set; }
        /// <summary>
        /// 是否专管
        /// </summary>
        public bool IsMaster { get; set; }

        /// <summary>
        /// 二维码或条形码
        /// </summary> 
        public string BarCode { get; set; }

        /// <summary>
        /// 批次
        /// </summary> 
        public string BatchNo { get; set; }



        /// <summary>
        /// 所属位置
        /// </summary>
        public int LocationId { get; set; }
        /// <summary>
        /// 试剂的具体存储位置    
        /// </summary>
        public string LocationName { get; set; }
        //public virtual Location Location { get; set; }

        /// <summary>
        /// 所属仓库
        /// </summary>
        public int WarehouseId { get; set; }


        /// <summary>
        /// 容量  规格 冗余自试剂基本信息
        /// </summary>
        public string Capacity { get; set; }


        /// <summary>   
        /// 容量单位   规格单位 冗余自试剂基本信息
        /// </summary> 
        public string CapacityUnit { get; set; }

        public string Guige => $"{this.Capacity}({this.CapacityUnit})";


        #region 供应商 冗余自试剂基本信息
        /// <summary>
        /// 供应商 冗余自试剂基本信息
        /// 添加的时候默认带着试剂基本信息的供应商，但是允许用户修改
        /// </summary>
        public int? SupplierCompanyId { get; set; }
        public string SupplierCompanyName { get; set; }
        #endregion


        #region 生产商 冗余自试剂基本信息
        /// <summary>
        /// 生产商 冗余自试剂基本信息
        /// 添加的时候默认带着试剂基本信息的生产商，但是允许用户修改
        /// </summary>
        public int? ProductionCompanyId { get; set; }
        public string ProductionCompanyName { get; set; }
        #endregion

        /// <summary>
        /// 库存状态
        /// </summary>
        public ReagentStockStatusEnum StockStatus { get; set; }
        public string StockStatusString => this.StockStatus.ToString();

        /// <summary>
        /// 试剂数量   普通专用
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 实际在库数量  普通专用
        /// </summary>
        public int RealAmount { get; set; }

        public bool CanStaockIn {
            get
            {
                if (this.IsMaster)
                {
                    return StockStatus == ReagentStockStatusEnum.待入库 || StockStatus == ReagentStockStatusEnum.离库;
                }
                return true;
            }   
            set => canStaockIn = value; } //=> StockStatus == ReagentStockStatusEnum.待入库 || StockStatus == ReagentStockStatusEnum.离库;

        #region 试剂入库相关人和时间
        /// <summary>
        /// 最早试剂入库时间
        /// 第一次入库时间
        /// </summary>
        public DateTime? FirstStockInTime { get; set; }

        /// <summary>
        /// 最新入库时间
        /// </summary>
        public DateTime? LatestStockInTime { get; set; }
        /// <summary>
        /// 最新一次入库人 
        /// </summary> 
        public string LatestStockInUserName { get; set; }
        #endregion

        #region 试剂领用相关人
        /// <summary>
        /// 最新一次出库时间
        /// 最新一次被领用事件
        /// </summary>
        public DateTime? LatestStockOutTime { get; set; }

        /// <summary>
        /// 最新一次出库人
        /// </summary> 
        public string LatestStockOutUserName { get; set; }
        #endregion

        #region 试剂回收人
        /// <summary>
        /// 回收时间
        /// 已用完时间
        /// </summary>
        public DateTime? RetrieveTime { get; set; }

        public string RetrieveUserName { get; set; }
        #endregion

        /// <summary>
        /// 针对哪个试剂
        /// </summary>
        public int ReagentId { get; set; }

        /// <summary>
        /// 针对哪个试剂的库存
        /// </summary> 
        public string ReagentCasNo { get; set; }
        public string ReagentNo { get; set; }
        public string ReagentCnName { get; set; }
        public string ReagentCnAliasName { get; set; }
        public string ReagentEnName { get; set; }
 

        /// <summary>
        /// 客户端入库确认
        /// </summary>
        public bool ClientConfirm { get; set; }
        public string ClientConfirmStr => this.ClientConfirm ? "是" : "否";
        /// <summary>
        /// 双人双锁
        /// </summary>
        public bool DoubleConfirm { get; set; }
        public string DoubleConfirmStr => this.DoubleConfirm ? "是" : "否";
        /// <summary>
        /// 安全属性
        /// 若该项选择，该试剂为专管试剂，也可不选，若该项不选择试剂为普通试剂   
        /// </summary>
        public SafeAttributes? ReagentSafeAttribute { get; set; }
        public string ReagentSafeAttributeString => this.ReagentSafeAttribute.HasValue ? this.ReagentSafeAttribute.ToString() : "-";

        public bool IsDeleted { get; set; }

        public string CreateUserName { get; set; }
    }
}
