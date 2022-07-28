using System.Collections.Generic;

namespace Guoxu.LabManager.Roles.Dto
{
    public class GetRoleForEditOutput
    {
        public RoleEditDto Role { get; set; }

        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }

        /// <summary>
        /// 已经有的仓库权限    
        /// </summary>
        //public List<WarehousePermissionDto> GrantedWarehousePermissions { get; set; }

        //public List<WarehousePermissionContainer> WarehousePermissions { get; set; }
    }
}