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
using GalaSoft.MvvmLight.Ioc;
using InfiStoone.LabClient.Tools;
using InfiStoone.LabClient.ViewModel;

namespace InfiStoone.LabClient.Pages.LoginChild
{
    /// <summary>
    /// FingerprintLogin.xaml 的交互逻辑
    /// </summary>
    public partial class FingerprintLogin : UserControl
    {
        public FingerprintLogin()
        {
            InitializeComponent();
            this.Loaded += FingerprintLogin_Loaded;
            this.Unloaded += FingerprintLogin_Unloaded;
        }

        private void FingerprintLogin_Unloaded(object sender, RoutedEventArgs e)
        {
            SimpleIoc.Default.GetInstance<MainViewModel>().ShowBackHome = false;
        }

        private void FingerprintLogin_Loaded(object sender, RoutedEventArgs e)
        {
            SimpleIoc.Default.GetInstance<MainViewModel>().ShowBackHome = true;
        }

        private void Home_OnClick(object sender, RoutedEventArgs e)
        {
            NavCtrl.NavToPage("LoginChild.LoginHome");
        }
    }
}
