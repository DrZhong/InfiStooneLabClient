using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Guoxu.LabManager.Domains;
using Guoxu.LabManager.Dto;

namespace Guoxu.LabManager.ReagentService.Dto;

public class ReagentOperateRecordDtoInputDto: PagedResultRequestFilterDto
{

    public string ReagentNo { get; set; }
    //public string ReagentCnName { get; set; }
    public string BarCode { get; set; }
    public string CreateUserName { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    /// <summary>
    /// 操作类型
    /// 入库、领用、回收
    /// </summary>
    public OperateTypeEnum? OperateType { get; set; }

    public int? WarehouseId { get; set; }

    public string CasNo { get; set; }
}


[AutoMapFrom(typeof(ReagentOperateRecord),typeof(NormalReagentOperateRecord))]
public class ReagentOperateRecordDto : CreationAuditedEntityDto<long>
{
    /// <summary>
    /// 操作时的重量，单位 g
    /// </summary>
    public decimal Weight { get; set; }

    /// <summary>
    /// 操作数量
    /// </summary>
    public int OperateAmount { get; set; }
    public int NormalReagentStockId { get; set; }

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
    /// <summary>
    /// 针对哪个试剂的库存
    /// </summary> 
    public string ReagentCasNo { get; set; }
    public string ReagentNo { get; set; }
    public string ReagentCnName { get; set; }
    public string ReagentCnAliasName { get; set; }
    public string ReagentEnName { get; set; }
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
}