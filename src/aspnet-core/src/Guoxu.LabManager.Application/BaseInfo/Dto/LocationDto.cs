using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper; 
using Guoxu.LabManager.Domains;
using Guoxu.LabManager.Dto;

namespace Guoxu.LabManager.BaseInfo.Dto;

public class GetLocationDto : PagedResultRequestFilterDto
{
    /// <summary>
    /// 所属仓库
    /// </summary>
    public int? WarehouseId { get; set; }

    /// <summary>
    /// 存储属性
    /// </summary>
    public StorageAttrEnum? StorageAttr { get; set; }
}

//[AutoMap(typeof(Location))]
public class LocationDto : CreationAuditedEntityDto 
{
    private List<LocationStorageAttrDto> _locationStorageAttr;

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

    public string WarehouseName { get; set; }

    public string CreateUserName { get; set; } 
    public bool IsActive { get; set; }
    /// <summary>
    /// 如果非0 则限制此货位的最大存货量
    /// </summary>
    public int CountLimit { get; set; }
    public List<LocationStorageAttrDto> LocationStorageAttr
    {
        get => _locationStorageAttr??(_locationStorageAttr=new List<LocationStorageAttrDto>());
        set => _locationStorageAttr = value;
    }
}

[AutoMap(typeof(LocationStorageAttr))]
public class LocationStorageAttrDto : EntityDto
{


    public int LocationId { get; set; } 
        
    /// <summary>
    /// 存储属性
    /// </summary>
    public StorageAttrEnum StorageAttr { get; set; }

    public string StorageAttrToString => this.StorageAttr.ToString();


    /// <summary>
    /// 是否选中、是否可用
    /// </summary>
    public bool IsActive { get; set; }
}