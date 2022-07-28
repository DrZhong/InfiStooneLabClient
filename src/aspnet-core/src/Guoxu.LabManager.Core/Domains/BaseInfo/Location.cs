using System.Collections.Generic;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Guoxu.LabManager.Domains;

/// <summary>
/// 库位管理
/// </summary>
public class Location:CreationAuditedEntity,IHasCreateUserName,ISoftDelete,IPassivable, IMustHaveWarehouseType
{
    private ICollection<LocationStorageAttr> _locationStorageAttr;

    /// <summary>
    /// 货架
    /// </summary>
    public string ShelfNo { get; set; }
    /// <summary>
    /// 层
    /// </summary>
    public int Row { get; set; }
    /// <summary>
    /// 列
    /// </summary>
    public int Column { get; set; }

    /// <summary>
    /// 库位名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 顺序
    /// </summary>
    public int Sort { get; set; }
    /// <summary>
    /// 所属仓库
    /// </summary>
    public int WarehouseId { get; set; }
    public virtual Warehouse Warehouse { get; set; }

    public string CreateUserName { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }

    /// <summary>
    /// 如果非0 则限制此货位的最大存货量
    /// </summary>
    public int CountLimit { get; set; }

    public WarehouseType WarehouseType { get; set; }

    public virtual ICollection<LocationStorageAttr> LocationStorageAttr
    {
        get => _locationStorageAttr ??(_locationStorageAttr = new List<LocationStorageAttr>());
        set => _locationStorageAttr = value;
    }
}


/// <summary>
/// 库位的存储属性
/// </summary>
public class LocationStorageAttr:Entity
{


    public int LocationId { get; set; }
    public virtual Location Location { get; set; }

    /// <summary>
    /// 存储属性
    /// </summary>
    public StorageAttrEnum StorageAttr { get; set; }


    /// <summary>
    /// 是否选中、是否可用
    /// </summary>
    public bool IsActive { get; set; }
}