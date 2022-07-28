using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiStoone.CodePrint
{
    public class SettingDto
    {
        /// <summary>
        /// 画布宽 单位 毫米
        /// </summary>
        public int PageWidth { get; set; }

        /// <summary>
        /// 画布高  单位毫米
        /// </summary>
        public int PageHeight { get; set; }

        /// <summary>
        /// 二维码宽度
        /// </summary>
        //public int qrCodeWidth { get; set; }
        //  "barCodeWidth": 300,
        // "barCodeHight": 150,
        public int BarCodeWidth { get; set; }

        public int BarCodeHight { get; set; }

        /// <summary>
        /// 上边距
        /// </summary>
        public int Margin { get; set; }


 


        public string printerName { get; set; }
    }
}
