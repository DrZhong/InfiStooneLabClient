using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper; 
using Guoxu.LabManager.Domains;

namespace Guoxu.LabManager.Roles.Dto;

public class WarehousePermissionContainer
{
    private List<WarehousePermissionContainerItem> _item;
    public string WareHouseName { get; set; }
    public int WareHouseId { get; set; }

    public int RoleId { get; set; }

    public List<WarehousePermissionContainerItem> Item
    {
        get => _item??=new List<WarehousePermissionContainerItem>();
        set => _item = value;
    }
}

public class WarehousePermissionContainerItem
{
    /// <summary>
    /// 权限名称
    /// </summary>
    public WarehousePermissionEnum Permission { get; set; }

    public string PermissionName => this.Permission.ToString();

    /// <summary>
    /// 是否拥有
    /// </summary>
    public bool IsGranted { get; set; }
}

[AutoMap(typeof(WarehousePermission))]
public class WarehousePermissionDto : EntityDto
{
    public int RoleId { get; set; } 

    /// <summary>
    /// 所属仓库
    /// </summary>
    public int WarehouseId { get; set; } 
    /// <summary>
    /// 拥有的权限
    /// </summary>
    public WarehousePermissionEnum Permission { get; set; }
}