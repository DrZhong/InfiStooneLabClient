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
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using HandyControl.Controls;
using InfiStoone.LabClient.Runtime;
using InfiStoone.LabClient.Services;
using InfiStoone.LabClient.Tools;
using InfiStoone.LabClient.ViewModel;

namespace InfiStoone.LabClient.Pages.LoginChild
{
    /// <summary>
    /// AccountLogin.xaml 的交互逻辑
    /// </summary>
    public partial class AccountLogin : UserControl
    {
        public AccountLogin()
        {
            InitializeComponent();
 
            this.Loaded += AccountLogin_Loaded;
            this.Unloaded += AccountLogin_Unloaded;
            Task.Factory.StartNew(() => {
                System.Threading.Thread.Sleep(300);
                this.Dispatcher.Invoke(() => {
                    this.UserName.Focus();
                });
            });
        }

        private void AccountLogin_Unloaded(object sender, RoutedEventArgs e)
        {
            SimpleIoc.Default.GetInstance<MainViewModel>().ShowBackHome = false; 
           
        }
         

        private void AccountLogin_Loaded(object sender, RoutedEventArgs e)
        { 
            SimpleIoc.Default.GetInstance<MainViewModel>().ShowBackHome = true; 
        }
         
     

        private void Login_OnClick(object sender, RoutedEventArgs e)
        {
           
          
            this.LoginToSystem();
        }

        private void LoginToSystem()
        {
            DrawerBottom.IsOpen = false;
            var userName = this.UserName.Text;
            var pwd = this.PassWord.Password;
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(pwd))
            {
                NotifyHelper.Warn("请输入用户名和密码");
                return;
            }
            SimpleIoc.Default.GetInstance<DataService>().AccountLogin(userName, pwd);
            //登陆成功
            this.PassWord.Password = "";
            NavCtrl.NavToPage("Home");
        }

        private void Home_OnClick(object sender, RoutedEventArgs e)
        {
            NavCtrl.NavToPage("LoginChild.LoginHome");
        } 
        private void PassWord_OnGotFocus(object sender, RoutedEventArgs e)
        { 
            DrawerBottom.IsOpen = true; 
        }

        private void PassWord_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                LoginToSystem();
            }
        }
    }
}
