using MiniExcelLibs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guoxu.LabManager.BaseInfo.Dto
{
    public class ReagentExcelDto
    {
        [ExcelColumnName("编号")]
        public string No { get; set; }

        /// <summary>
        /// 试剂状态
        /// </summary>
        [ExcelColumnName("状态")]
        public string ReagentStatus { get; set; }

        /// <summary>
        /// 试剂类型
        /// </summary>
        [ExcelColumnName("试剂类型")]
        public string ReagentCatalog { get; set; }
        [ExcelColumnName("CAS号")]
        public string CasNo { get; set; }
        [ExcelColumnName("中文名")]
        public string CnName { get; set; }
        [ExcelColumnName("拼音码")]
        public string PinYinCode { get; set; }
        [ExcelColumnName("中文别名")]
        public string CnAliasName { get; set; }
        [ExcelColumnName("英文名")]
        public string EnName { get; set; }
        [ExcelColumnName("安全属性")]
        public string? SafeAttribute { get; set; }
        [ExcelColumnName("参考价格")]
        public decimal Price { get; set; }
        [ExcelColumnName("存储属性")]
        public string StorageAttr { get; set; }
        [ExcelColumnName("纯度")]
        public string Purity { get; set; }
        [ExcelColumnName("容量")]
        public string Capacity { get; set; }
        [ExcelColumnName("容量单位")]
        public string CapacityUnit { get; set; }
        [ExcelColumnName("存储条件")]
        public string StorageCondition { get; set; }
        [ExcelColumnName("库存预警")]
        public int InventoryWarning { get; set; }
        [ExcelColumnName("供应商")]
        public string SupplierCompanyName { get; set; }
        [ExcelColumnName("生产商")]
        public string ProductionCompanyName { get; set; }
    }
}
