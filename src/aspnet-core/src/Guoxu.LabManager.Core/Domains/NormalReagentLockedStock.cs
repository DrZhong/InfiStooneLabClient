using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guoxu.LabManager.Domains
{
    /// <summary>
    /// 普通试剂 被锁住的库存
    /// </summary>
    public class NormalReagentLockedStock:Entity
    {
        /// <summary>
        /// 哪个试剂
        /// </summary>
        public int ReagentId { get; set; }

        /// <summary>
        /// 在哪个库位
        /// </summary>
        public int LocationId { get; set; }

        /// <summary>
        /// 目前被锁住了多少库存
        /// </summary>
        public int LockAccount { get; set; }

    }
}
