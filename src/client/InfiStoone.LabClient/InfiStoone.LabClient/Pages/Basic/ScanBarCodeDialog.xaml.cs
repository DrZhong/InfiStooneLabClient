using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InfiStoone.LabClient.Pages.Basic
{
    /// <summary>
    /// ScanBarCodeDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ScanBarCodeDialog //: UserControl
    {
        public ScanBarCodeDialog()
        {
            InitializeComponent();
            Task.Factory.StartNew(() => {
                Thread.Sleep(300);
                this.Dispatcher.Invoke(() => {
                    this.BarCodeTxt.Focus();
                });
            });
            
        }
    }
}
