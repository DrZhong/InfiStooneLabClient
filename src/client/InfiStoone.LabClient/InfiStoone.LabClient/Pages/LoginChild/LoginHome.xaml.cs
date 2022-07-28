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
using GalaSoft.MvvmLight.Messaging;
using HandyControl.Controls;
using InfiStoone.LabClient.Tools;
using InfiStoone.LabClient.ViewModel;

namespace InfiStoone.LabClient.Pages.LoginChild
{
    /// <summary>
    /// LoginHome.xaml 的交互逻辑
    /// </summary>
    public partial class LoginHome : UserControl
    {
        public LoginHome()
        {
            InitializeComponent();
           
        } 
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            NavCtrl.NavToPage("LoginChild.AccountLogin");
            //Messenger.Default.Send<string>("Pages.LoginChild.AccountLogin", MessengerToken.LoadShowContent);
        }

        private void Finger_OnClick(object sender, RoutedEventArgs e)
        {
            //Dialog.Show(new SelectWarehouse());
             NavCtrl.NavToPage("LoginChild.FingerprintLogin");
            //Messenger.Default.Send<string>("Pages.LoginChild.FingerprintLogin", MessengerToken.LoadShowContent);
        }

        private void Face_OnClick(object sender, RoutedEventArgs e)
        {
            NavCtrl.NavToPage("LoginChild.FaceLogin");
            //Messenger.Default.Send<string>("Pages.LoginChild.FaceLogin", MessengerToken.LoadShowContent);
        }
    }
}
