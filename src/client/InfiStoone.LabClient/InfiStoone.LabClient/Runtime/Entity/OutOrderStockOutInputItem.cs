using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiStoone.LabClient.Runtime.Entity
{
    public class OutOrderStockOutInputItem: ViewModelBase
    { 
        public string BarCode { get; set; }
         
        public int Account { get; set; }
    }
}
