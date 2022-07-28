using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using InfiStoone.LabClient.Tools;

namespace InfiStoone.LabClient.Pages
{
    /// <summary>
    /// Home.xaml 的交互逻辑
    /// </summary>
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
        }

        private void SaveCard_OnClick(object sender, RoutedEventArgs e)
        {
            NotifyHelper.Info("你点击了安全卡");
        }

        private void LoginOut_OnClick(object sender, RoutedEventArgs e)
        {
            MessageHelper.Confirm("你确定要退出吗？", res =>
            {
                if (res)
                {
                    Application.Current.Shutdown(0);
                }
            });
            
        }
    }
}
