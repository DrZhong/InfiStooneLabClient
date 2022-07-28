 using GalaSoft.MvvmLight.Messaging;
using HandyControl.Controls;
using InfiStoone.LabClient.Pages;
using InfiStoone.LabClient.Runtime;
using InfiStoone.LabClient.Tools;
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

namespace InfiStoone.LabClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow  
    {
        private const bool FullScreen = true;
        public MainWindow()
        {
            InitializeComponent();
            this.IsFullScreen = FullScreen;
          
            this.Loaded += MainWindow_Loaded;

            
        }


        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var cinfigModel = Config.ConfigModel;
            //检测密钥
            if (string.IsNullOrEmpty(cinfigModel.Token))
            {
                //输入密钥
                //没有设置智能药盒

                var result= new EnterLucene().ShowDialog();
                if(result==null || result.Value == false)
                {
                    //关闭应用程序
                    Application.Current.Shutdown(0);
                }
            }
            //检测密钥
            var checkResult= Utll.CheckLucense();
            if (checkResult == false)
            {
                Task.Factory.StartNew(() => {
                    NotifyHelper.Warn("请勿手动修改授权文件，3秒后将自动关闭程序，请重新打开程序输入授权文件");
                });
              
                Config.ConfigModel.Token = null;
                Config.SaveConfig();
                //授权失败
                //关闭应用程序
                Thread.Sleep(1000 * 3);
                Application.Current.Shutdown(0);

            }




            if (cinfigModel.WareHouse == null)
            {
                //没有设置智能药盒

                new SelectWarehouse().ShowDialog();
            }
            else
            {
                Session.SelectedWareHouse = cinfigModel.WareHouse;
                Messenger.Default.Send(cinfigModel.WareHouse, MessengerToken.HouseWareSelectChange);
            }

 
        }

        private void LoginOut_Click(object sender, RoutedEventArgs e)
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
