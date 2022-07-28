using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Runtime.Validation;
using Abp.UI;
using Guoxu.LabManager.Domains;
using Guoxu.LabManager.Dto;

namespace Guoxu.LabManager.ReagentService.Dto;

public class GetAllMasterInputDto: PagedResultRequestFilterDto
{
    public string BarCode { get; set; }
    public string ReagentCasNo { get; set; }
    public string ReagentNo { get; set; }
    public string BatchNo { get; set; }
    public string SupplierCompanyName { get; set; }
    public int? WarehouseId { get; set; }
    public ReagentStockStatusEnum? StockStatus { get; set; }
}

[AutoMapFrom(typeof(ReagentStock))]
public class ReagentStockListDto : CreationAuditedEntityDto
{
    /// <summary>
    /// 重量，单位 g
    /// </summary>
    public decimal Weight { get; set; }

    /// <summary>
    /// 被 哪个出库单锁定了
    /// 锁定的试剂无法出库
    /// </summary> 
    public long? LockedOrderId { get; set; }
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
    /// <summary>
    /// 二维码或条形码
    /// </summary>
    public string BarCode { get; set; }

    /// <summary>
    /// 批次
    /// </summary>
    public string BatchNo { get; set; }


    public string CasNo { get; set; }

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
    /// 试剂的具体存储位置    
    /// </summary>
    public string LocationName { get; set; }
    //public virtual Location Location { get; set; }

    /// <summary>
    /// 所属仓库
    /// </summary>
    public int WarehouseId { get; set; }
    public string WarehouseName { get; set; }


    /// <summary>
    /// 容量  规格 冗余自试剂基本信息
    /// </summary>
    public string Capacity { get; set; }


    /// <summary>   
    /// 容量单位   规格单位 冗余自试剂基本信息
    /// </summary> 
    public string CapacityUnit { get; set; }


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
    /// 安全属性
    /// 若该项选择，该试剂为专管试剂，也可不选，若该项不选择试剂为普通试剂
    /// </summary>
    public SafeAttributes? ReagentSafeAttribute { get; set; }

    /// <summary>
    /// 存储属性 
    /// </summary>
    public StorageAttrEnum ReagentStorageAttr { get; set; }

    /// <summary>
    /// 纯度  字典
    /// </summary>
    public string ReagentPurity { get; set; }

    public bool IsDeleted { get; set; }

    [MaxLength(512)]
    public string CreateUserName { get; set; }

    /// <summary>
    /// 参考价格
    /// </summary>
    public decimal Price { get; set; }
}


[AutoMap(typeof(ReagentStock))]
public class ReagentStockDto : CreationAuditedEntityDto
{
    /// <summary>
    /// 重量，单位 g
    /// </summary>
    public decimal Weight { get; set; }
    /// <summary>
    /// 价格
    /// </summary>
    public decimal Price { get; set; }
    /// <summary>
    /// 是否专管
    /// </summary>
    public bool IsMaster { get; set; }
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
    public string WarehouseName { get; set; }

    /// <summary>
    /// 容量  规格 冗余自试剂基本信息
    /// </summary>
    public string Capacity { get; set; }


    /// <summary>   
    /// 容量单位   规格单位 冗余自试剂基本信息
    /// </summary>
    [MaxLength(512)]
    public string CapacityUnit { get; set; }


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
    public string ReagentPurity { get; set; }
    /// <summary>
    /// 客户端入库确认
    /// </summary>
    public bool ClientConfirm { get; set; }
    /// <summary>
    /// 双人双锁
    /// </summary>
    public bool DoubleConfirm { get; set; }
    /// <summary>
    /// 安全属性
    /// 若该项选择，该试剂为专管试剂，也可不选，若该项不选择试剂为普通试剂
    /// </summary>
    public SafeAttributes? ReagentSafeAttribute { get; set; }

    public bool IsDeleted { get; set; }

    [MaxLength(512)]
    public string CreateUserName { get; set; }

    /// <summary>
    /// 试剂数量   普通专用
    /// </summary>
    public int Amount { get; set; }

    /// <summary>
    /// 实际在库数量  普通专用
    /// </summary>
    public int RealAmount { get; set; }
}

[AutoMapTo(typeof(ReagentStock))]
public class CreateReagentStockDto:ICustomValidate
{
    /// <summary>
    /// 生成数量
    /// </summary>
    //public int Num { get; set; }    

    public List<string> Codes { get; set; } 

    /// <summary>
    /// 批次
    /// </summary>
    [MaxLength(512)]
    [Required]
    public string BatchNo { get; set; }

    /// <summary>
    /// 所属位置
    /// </summary>
    public int LocationId { get; set; }


    /// <summary>
    /// 供应商 冗余自试剂基本信息
    /// 添加的时候默认带着试剂基本信息的供应商，但是允许用户修改
    /// </summary>
    public int? SupplierCompanyId { get; set; }
    /// <summary>
    /// 生产商 冗余自试剂基本信息
    /// 添加的时候默认带着试剂基本信息的生产商，但是允许用户修改
    /// </summary>
    public int? ProductionCompanyId { get; set; }



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
    public DateTime? ExpirationDate { get; set; }
    public bool ClientConfirm { get; set; } 
    public bool DoubleConfirm { get; set; } 

    /// <summary>
    /// 针对哪个试剂
    /// </summary>
    public int ReagentId { get; set; }


    /// <summary>
    /// 参考价格
    /// 如果为0则 使用试剂的参考价格
    /// </summary>
    public decimal Price { get; set; }

    public void AddValidationErrors(CustomValidationContext context)
    {
        if (!ProductionDate.HasValue && !ExpirationDate.HasValue)
        {
            throw new UserFriendlyException("生产日期和过期日期必须输入一个");
        }

        if (Codes.IsNullOrEmpty())
        {
            throw new UserFriendlyException("请提供条码！");
        }
    }
}