using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiStoone.LabClient.Runtime.Entity
{
    public enum WarehouseType
    {
        试剂 = 1,
        耗材 = 2,
        办公 = 3
    }

    public class WareHouseDto: EntityBase
    {
        /// <summary>
        /// 仓库类型
        /// </summary>
        public WarehouseType WarehouseType { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary> 
        public string Name { get; set; }

        /// <summary>
        /// 仓库代码
        /// </summary> 
        public string Code { get; set; }

        /// <summary>
        /// 仓库地址
        /// </summary> 
        public string Address { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary> 
        public string Phone { get; set; }



        /// <summary>
        /// 是否弃用
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 创建者
        /// </summary> 
        public string CreateUserName { get; set; }
    }
}
