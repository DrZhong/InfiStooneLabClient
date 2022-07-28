using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using System.Globalization;
using Guoxu.LabManager.Authorization.Users;
using System.Collections.Generic;

namespace Guoxu.LabManager.Domains;

/// <summary>
/// 试剂库存状态
/// </summary>
public enum ReagentStockStatusEnum
{
    待入库 = 0,
    在库 = 1,
    /// <summary>
    /// 领用
    /// </summary>
    离库 = 2,
    /// <summary>
    /// 瓶子回收后 变成已用完
    /// </summary>
    已用完 = 3
}

public enum OperateTypeEnum
{
    入库=1,
    /// <summary>
    /// 领用就是出库
    /// </summary>
    领用=2,
    归还 = 3 ,
    /// <summary>
    /// 出库就是回收
    /// </summary>
    回收 = 4
}

/// <summary>
/// 专管试剂库存
/// </summary>
[AutoMapFrom(typeof(ReagentStock))]
public class ReagentStock : CreationAuditedEntity, ISoftDelete//, IHasCreateUserName
{
    private ICollection<ReagentStockAudit> reagentStockAudits;

    /// <summary>
    /// 重量，单位 g
    /// </summary>
    public decimal Weight { get; set; }

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
    /// 安全属性
    /// 若该项选择，该试剂为专管试剂，也可不选，若该项不选择试剂为普通试剂
    /// </summary>
    public SafeAttributes? SafeAttribute { get; set; }


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

    #region 库存状态
    /// <summary>
    /// 库存状态
    /// </summary>
    public ReagentStockStatusEnum StockStatus { get; set; } 
    #endregion

    #region  针对专管试剂 在被领用后 一天后没有归还的话 就发站内信
    /// <summary>
    /// 是否已经提醒过了
    /// 针对专管试剂 在被领用后 一天后没有归还的话 就发站内信
    /// </summary>
    public bool IsNoticed { get; set; } 
    #endregion


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

    public long? LatestStockOutUserId { get; set; }
    #endregion

    #region 试剂回收人
    /// <summary>
    /// 回收时间
    /// 已用完时间
    /// </summary>
    public DateTime? RetrieveTime { get; set; }
    [MaxLength(512)]
    public string RetrieveUserName { get; set; }
    #endregion


    #region 双锁！
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
    public bool ClientConfirmed { get; set; }
    /// <summary>
    /// 双人双锁   审核是否通过
    /// </summary>
    public bool DoubleConfirmed { get; set; } 
    #endregion


    public virtual ICollection<ReagentStockAudit> ReagentStockAudits {
        get => reagentStockAudits ?? (reagentStockAudits = new List<ReagentStockAudit>());
        set => reagentStockAudits = value; 
    }

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

    

    public bool IsDeleted { get; set; }

    [MaxLength(512)]
    public string CreateUserName { get; set; }

    /// <summary>
    /// 被 哪个出库单锁定了
    /// 锁定的试剂无法出库
    /// </summary> 
    public long? LockedOrderId { get; set; }

    /// <summary>
    /// 参考价格
    /// </summary>
    public decimal Price { get; set; }
}

public enum ReagentStockAuditTypeEnum
{
    入库确认 =1,
    入库双人双锁 =2,
    出库确认=3,
    出库双人双锁=4
}

/// <summary>
/// 专管试剂审核
/// 入库确认、出入库双人双审核
/// </summary>
public class ReagentStockAudit:CreationAuditedEntity
{
    public int ReagentStockId { get; set; } 
    public virtual ReagentStock ReagentStock { get; set; }
    /// <summary>
    /// 审核类型
    /// </summary>
    public ReagentStockAuditTypeEnum ReagentStockAuditType { get; set; }

    public string AuditUserName { get; set; }
    public long AuditUserId { get; set; }
    public virtual User AuditUser { get; set; }
}   

/// <summary>
/// 专管试剂库存履历
/// </summary>
public class ReagentStockHistory : CreationAuditedEntity, ISoftDelete //, IHasCreateUserName
{
    /// <summary>
    /// 履历内容
    /// </summary>
    public string Content { get; set; } 


    public int ReagentStockId { get; set; }
    public virtual ReagentStock ReagentStock { get; set; }

    public bool IsDeleted { get; set; }
    [MaxLength(512)]
    public string CreateUserName { get; set; }
}



/// <summary>
/// 专管操作记录
/// </summary>
public class ReagentOperateRecord:CreationAuditedEntity<long>
{
    public ReagentOperateRecord()
    {
        GregorianCalendar gc = new GregorianCalendar();
        this.Week= gc.GetWeekOfYear(DateTime.Today, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
    }


    /// <summary>
    /// 操作时的重量，单位 g
    /// </summary>
    public decimal Weight { get; set; }

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
    public virtual Warehouse Warehouse { get; set; }


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
    /// 安全属性
    /// 若该项选择，该试剂为专管试剂，也可不选，若该项不选择试剂为普通试剂
    /// </summary>
    public SafeAttributes? SafeAttribute { get; set; }

    /// <summary>
    /// 操作类型
    /// 入库、领用、回收
    /// </summary>
    public OperateTypeEnum OperateType { get; set; }

    /// <summary>
    /// 针对哪个试剂
    /// </summary>
    public int ReagentId { get; set; }
    /// <summary>
    /// 针对哪个试剂的库存
    /// </summary>
    public virtual Reagent Reagent { get; set; }
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