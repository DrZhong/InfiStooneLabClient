using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiStoone.LabClient.Runtime.Entity
{
    public enum WarehousePermissionEnum
    {
        库存查询 = 1,
        试剂入库 = 2,
        试剂领用 = 3,
        试剂归还 = 4,
        出库单 = 5
    }
    public class WarehousePermissionDto:EntityBase
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
}
