using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Guoxu.LabManager.Domains;

/// <summary>
/// 字典
/// </summary>
public class Dict:Entity,IHasCreateUserName,ISoftDelete, IMustHaveWarehouseType
{
    /// <summary>
    /// 试剂纯度
    /// </summary>
    public const string ReagentPurity = "ReagentPurity";

    /// <summary>
    /// 存储条件
    /// </summary>
    public const string ReagentStorageCondition = "ReagentStorageCondition";

    /// <summary>
    /// 容量单位
    /// </summary>
    public const string ReagentCapacityUnit = "ReagentCapacityUnit";
    /// <summary>
    /// 健
    /// </summary>
    [Required]
    public string Name { get; set; }


    public string DisplayName { get; set; }


    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; set; }   

    public string DisplayValue { get; set; }

    /// <summary>
    /// 顺序
    /// </summary>
    public int Sort { get; set; }

    public int? ParentId { get; set; }
    public virtual Dict Parent { get; set; }

    /// <summary>
    /// 子节点
    /// </summary>
    public virtual ICollection<Dict> Child { get; set; }


    /// <summary>
    /// 所属供应商类型
    /// </summary>
    public WarehouseType WarehouseType { get; set; }

    public string CreateUserName { get; set; }
    public bool IsDeleted { get; set; }
}