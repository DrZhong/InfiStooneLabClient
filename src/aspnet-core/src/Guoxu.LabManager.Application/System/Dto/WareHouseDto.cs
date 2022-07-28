using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Guoxu.LabManager.Domains;
using Guoxu.LabManager.Dto;

namespace Guoxu.LabManager.System.Dto;

public class SetMasterDto
{
    public string UserPhone  { get; set; }
    public int WarehouseId { get; set; }
}

[AutoMap(typeof(Warehouse))]
public class WareHouseDto : CreationAuditedEntityDto
{
    public ZhuanGuanNotifySettingEnum ZhuanGuanNotifySetting { get; set; }
    /// <summary>
    /// 提醒间隔周期  单位天
    /// </summary>
    public int NotifySettingIntervalHour { get; set; }
    /// <summary>
    /// 仓库类型
    /// </summary>
    public WarehouseType WarehouseType { get; set; }

    /// <summary>
    /// 仓库管理员
    /// </summary>
    public long? MasterUserId { get; set; }
    public string MasterUserName { get; set; }
    public string MasterUserUserName { get; set; }


    /// <summary>
    /// 仓库名称
    /// </summary>
    [MaxLength(64)]
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// 仓库代码
    /// </summary>
    [MaxLength(512)]
    public string Code { get; set; }

    /// <summary>
    /// 仓库地址
    /// </summary>
    [MaxLength(1024)]
    public string Address { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    [MaxLength(64)]
    public string Phone { get; set; }

     

    /// <summary>
    /// 是否弃用
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// 创建者
    /// </summary>
    [MaxLength(512)]
    public string CreateUserName { get; set; }
}