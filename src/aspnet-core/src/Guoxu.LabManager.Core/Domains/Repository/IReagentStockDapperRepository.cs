using Abp.Dapper.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guoxu.LabManager.Domains.Repository
{
    public class HomeMasterDto
    {
        /// <summary>
        /// 专管 今日领用
        /// </summary>
        public int TodayStockOutCount { get; set; }

        /// <summary>
        /// 专管 今日归还
        /// </summary>
        public int TodayStockBackCount { get; set; }

        /// <summary>
        /// 专管 今日入库
        /// </summary>
        public int TodayStockInCount { get; set; }

        /// <summary>
        /// 专管今日回收
        /// </summary>
        public int TodayStockRetrieveCount { get; set; }

        /// <summary>
        /// 库存预警
        /// </summary>
        public int RegentInventoryWarningCount { get; set; }

        /// <summary>
        /// 库存过期
        /// </summary>
        public int ExpirationCount { get; set; }

    }

    public class HomeNormalDto
    {
        /// <summary>
        /// 专管 今日领用
        /// </summary>
        public int TodayStockOutCount { get; set; }

        /// <summary>
        /// 专管 今日归还
        /// </summary>
        public int TodayStockBackCount { get; set; }

        /// <summary>
        /// 专管 今日入库
        /// </summary>
        public int TodayStockInCount { get; set; }

        /// <summary>
        /// 库存预警
        /// </summary>
        public int RegentInventoryWarningCount { get; set; }


        public int ExpirationCount { get; set; }
    }

    public interface IReagentStockDapperRepository: IDapperRepository<ReagentStock>
    {
        Task<HomeMasterDto> GetHomeMaster();

        Task<HomeNormalDto> GetHomeNormal();
    }
}
