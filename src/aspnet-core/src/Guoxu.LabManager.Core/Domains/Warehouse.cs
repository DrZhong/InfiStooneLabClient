using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Guoxu.LabManager.Authorization.Users;

namespace Guoxu.LabManager.Domains;

public enum WarehouseType
{
    试剂 = 1,
    耗材=2,
    办公=3
}

/// <summary>
/// 专管没有归还提醒周期
/// </summary>
public enum ZhuanGuanNotifySettingEnum
{
    不提醒,
    提醒一次,
    周期性提醒
}

/// <summary>
/// 仓库
/// </summary>
public class Warehouse:CreationAuditedEntity,ISoftDelete,IPassivable,IHasCreateUserName
{
    /// <summary>
    /// 仓库管理员
    /// </summary>
    public long? MasterUserId { get; set; }
    public virtual User MasterUser   { get; set; }

    /// <summary>
    /// 仓库类型
    /// </summary>
    public WarehouseType WarehouseType { get; set; }    
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


    public bool IsDeleted { get; set; }

    /// <summary>
    /// 是否弃用
    /// </summary>
    public bool IsActive { get; set; } = true;


    public ZhuanGuanNotifySettingEnum ZhuanGuanNotifySetting { get; set; }
    /// <summary>
    /// 提醒间隔周期  单位天
    /// </summary>
    public int NotifySettingIntervalHour { get; set; }

    /// <summary>
    /// 创建者
    /// </summary>  
    [MaxLength(512)]
    public string CreateUserName { get; set; }
}