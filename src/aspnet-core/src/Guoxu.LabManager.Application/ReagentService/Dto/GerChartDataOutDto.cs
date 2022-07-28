using Guoxu.LabManager.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guoxu.LabManager.ReagentService.Dto
{
    public class GerChartDataIntputDto :Abp.Runtime.Validation.IShouldNormalize
    {
        /// <summary>
        /// A-今天，B-昨天，C-最近7天，D-最近30天
        /// </summary>
        public string DateCurrent { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 分组
        /// 0-按时，1-按日，2-按周，3-按月
        /// </summary>
        public int GroupBy { get; set; }
        /// <summary>
        /// 统计类型
        /// true-专管，false-普通
        /// </summary>
        public bool MasterType { get; set; }

        public SafeAttributes? SafeAttributes { get; set; }

        /// <summary>
        /// 具体查询哪个试剂
        /// </summary>
        public int? ReagentId { get; set; }

        public void Normalize()
        {
            if (!string.IsNullOrEmpty(DateCurrent))
            {
                switch (DateCurrent)
                {
                    case "A":
                        this.StartDate = DateTime.Today;
                        this.EndDate = DateTime.Today.AddDays(1);
                        break;
                    case "B":
                        this.StartDate = DateTime.Today.AddDays(-1);
                        this.EndDate = DateTime.Today;
                        break;
                    case "C":
                        this.StartDate = DateTime.Today.AddDays(-7);
                        this.EndDate = DateTime.Now;
                        break;
                    case "D":
                        this.StartDate = DateTime.Today.AddDays(-30);
                        this.EndDate = DateTime.Now;
                        break;
                }
            }
        }
    }
    public class GerChartDataOutDto
    {
        private List<int> stockIn;
        private List<int> stockOut;
        private List<int> stockBack;
        private List<int> stockRetrieve;

        public List<string> Time { get; set; }
        /// <summary>
        /// 试剂入库
        /// </summary>
        public List<int> StockIn { get => stockIn??=new List<int>(); set => stockIn = value; }
        /// <summary>
        /// 试剂领用
        /// </summary>
        public List<int> StockOut { get => stockOut ??= new List<int>(); set => stockOut = value; }
        /// <summary>
        /// 归还
        /// </summary>
        public List<int> StockBack { get => stockBack ??= new List<int>(); set => stockBack = value; }

        public List<int> StockRetrieve { get => stockRetrieve ??= new List<int>(); set => stockRetrieve = value; }
    }
}
