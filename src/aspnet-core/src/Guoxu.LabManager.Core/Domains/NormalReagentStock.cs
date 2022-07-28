using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guoxu.LabManager.Domains
{
    /// <summary>
    /// 普通试剂库存
    /// </summary>
    [Abp.AutoMapper.AutoMap(typeof(NormalReagentStock))]
    public class NormalReagentStock : CreationAuditedEntity, ISoftDelete//, IHasCreateUserName
    {

        /// <summary>
        /// 二维码或条形码
        /// </summary>
        [MaxLength(512)]
        public string BarCode { get; set; }


        /// <summary>
        /// 批次
        /// </summary>
        [MaxLength(512)]
        public string BatchNo { get; set; }

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? ProductionDate { get; set; }

        /// <summary>
        /// 保质月数
        /// </summary>
        public int ExpirationMonth { get; set; }

        /// <summary>
        /// 保质日期
        /// </summary>
        public DateTime ExpirationDate { get; set; }

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
        public virtual Warehouse Warehouse { get; set; }


        #region 供应商 冗余自试剂基本信息
        /// <summary>
        /// 供应商 冗余自试剂基本信息
        /// 添加的时候默认带着试剂基本信息的供应商，但是允许用户修改
        /// </summary>
        public int? SupplierCompanyId { get; set; }
        public virtual Company SupplierCompany { get; set; }
        public string SupplierCompanyName { get; set; }
        #endregion


        #region 生产商 冗余自试剂基本信息
        /// <summary>
        /// 生产商 冗余自试剂基本信息
        /// 添加的时候默认带着试剂基本信息的生产商，但是允许用户修改
        /// </summary>
        public int? ProductionCompanyId { get; set; }
        public virtual Company ProductionCompany { get; set; }
        public string ProductionCompanyName { get; set; }
        #endregion

        /// <summary>
        /// 库存状态
        /// </summary>
        public ReagentStockStatusEnum StockStatus { get; set; }

        #region 试剂入库相关人和时间
        /// <summary>
        /// 最早试剂入库时间
        /// 第一次入库时间
        /// </summary>
        public DateTime?  StockInTime { get; set; }


        public DateTime? LatestStockInTime { get; set; }

        /// <summary>
        ///  入库人 
        /// </summary>
        [MaxLength(512)]
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
        [MaxLength(512)]
        public string LatestStockOutUserName { get; set; }
        #endregion


        #region 试剂信息
        /// <summary>
        /// 针对哪个试剂
        /// </summary>
        public int ReagentId { get; set; }
        /// <summary>
        /// 针对哪个试剂的库存
        /// </summary>
        public virtual Reagent Reagent { get; set; }

        /// <summary>
        /// 冗余一下试剂的拼音码
        /// </summary>
        [MaxLength(512)]
        public string PinYinCode { get; set; }

        /// <summary>
        /// cas号
        /// </summary>
        [MaxLength(512)]
        public string CasNo { get; set; }
        #endregion
        public string CreateUserName { get; set; }

        /// <summary>
        /// 容量
        /// </summary>
        public string Capacity { get; set; }

        /// <summary>
        /// 试剂数量
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 实际在库数量
        /// </summary>
        public int RealAmount { get; set; }

        /// <summary>   
        /// 容量单位 
        /// </summary>
        [MaxLength(512)]
        public string CapacityUnit { get; set; }
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 参考价格
        /// </summary>
        public decimal Price { get; set; }
    }



    /// <summary>
    /// 普通试剂操作记录
    /// </summary>
    public class NormalReagentOperateRecord : CreationAuditedEntity<long>
    {
        public NormalReagentOperateRecord()
        {
            GregorianCalendar gc = new GregorianCalendar();
            this.Week = gc.GetWeekOfYear(DateTime.Today, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
        }

        public int NormalReagentStockId { get; set; }
        public virtual NormalReagentStock NormalReagentStock { get; set; }


        /// <summary>
        /// 操作数量
        /// </summary>
        public int OperateAmount { get; set; }

        /// <summary>
        /// 二维码或条形码
        /// </summary>
        [MaxLength(512)]
        public string BarCode { get; set; }

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

        public  string WarehouseName { get; set; } 


        /// <summary>
        /// 容量  规格 冗余自试剂基本信息
        /// </summary>
        public string Capacity { get; set; }


        /// <summary>   
        /// 容量单位   规格单位 冗余自试剂基本信息
        /// </summary>
        [MaxLength(512)]
        public string CapacityUnit { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        [MaxLength(512)]
        public string BatchNo { get; set; }
         

        /// <summary>
        /// 操作类型
        /// 入库、领用、回收
        /// </summary>
        public OperateTypeEnum OperateType { get; set; }

        /// <summary>
        /// 针对哪个试剂
        /// </summary>
        public int ReagentId { get; set; }
        ///// <summary>
        ///// 针对哪个试剂的库存
        ///// </summary>
        //public virtual Reagent Reagent { get; set; }
        public string ReagentCasNo { get; set; }
        //[MaxLength(512)]
        public string ReagentNo { get; set; }
 
         public string ReagentCnName {  get; set; }

        [MaxLength(1024)]
        public string ReagentEnName { get; set; }

        public ReagentCatalog ReagentReagentCatalog { get; set; }

        public string ReagentPinYinCode { get; set; } 

        //[MaxLength(1024)]
        //public string EnName { get; set; }
        public string CreateUserName { get; set; }


        /// <summary>
        /// 入库时间拆分
        /// 方便统计 
        /// </summary>
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        /// <summary>
        /// 周
        /// </summary>
        public int Week { get; set; }
    }

}
