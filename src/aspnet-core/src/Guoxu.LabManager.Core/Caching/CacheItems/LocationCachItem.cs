using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Guoxu.LabManager.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guoxu.LabManager.Caching.CacheItems
{
    [AutoMapFrom(typeof(Location))]
    public class LocationCacheItem : EntityDto
    {    /// <summary>
         /// 如果非0 则限制此货位的最大存货量
         /// </summary>
        public int CountLimit { get; set; }

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
        public string CreateUserName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public WarehouseType WarehouseType { get; set; }
    }
}
