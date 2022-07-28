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
    /// FaceLogin.xaml 的交互逻辑
    /// </summary>
    public partial class FaceLogin : UserControl
    {
        public FaceLogin()
        {
            InitializeComponent();
            this.Loaded += FaceLogin_Loaded;
            this.Unloaded += FaceLogin_Unloaded;
        }

        private void FaceLogin_Unloaded(object sender, RoutedEventArgs e)
        {
            SimpleIoc.Default.GetInstance<MainViewModel>().ShowBackHome = false;
        }

        private void FaceLogin_Loaded(object sender, RoutedEventArgs e)
        {
            SimpleIoc.Default.GetInstance<MainViewModel>().ShowBackHome = true;
        }

   
    }
}
