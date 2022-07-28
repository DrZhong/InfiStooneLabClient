using Abp.Domain.Entities;
using Guoxu.LabManager.Authorization.Roles;

namespace Guoxu.LabManager.Domains;


public enum WarehousePermissionEnum
{
    库存查询=1,
    试剂入库=2,
    试剂领用=3,
    试剂归还=4,
    出库单=5
}

/// <summary>
/// 角色仓库权限
/// </summary>
public class WarehousePermission:Entity
{
    public int RoleId { get; set; }
    public virtual Role Role { get; set; }

    /// <summary>
    /// 所属仓库
    /// </summary>
    public int WarehouseId { get; set; }
    public virtual Warehouse Warehouse { get; set; }

    /// <summary>
    /// 拥有的权限
    /// </summary>
    public WarehousePermissionEnum Permission { get; set; }

    public bool IsActive { get; set; }  



}