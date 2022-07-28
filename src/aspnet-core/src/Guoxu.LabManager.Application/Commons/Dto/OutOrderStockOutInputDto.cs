using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guoxu.LabManager.Commons.Dto
{
    public class OutOrderStockOutInputDto
    {
        private List<OutOrderStockOutInputItem> items;

        public int OrderId { get; set; }

        public int WarehouseId { get; set; }


        public List<OutOrderStockOutInputItem> Items 
        {
            get => items??(items=new List<OutOrderStockOutInputItem>()); 
            set => items = value; 
        }
    }

    public class OutOrderStockOutInputItem:IShouldNormalize
    {
        [Required]
        public string BarCode { get; set; }

        [Range(1,int.MaxValue,ErrorMessage ="至少大于1瓶试剂")]
        public int Account { get; set; }

        public void Normalize()
        {
            //移除空格
            this.BarCode = this.BarCode.Trim();
        }
    }
}
