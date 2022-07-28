using InfiStoone.LabClient.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InfiStoone.LabClient.Pages
{

    /// <summary>
    /// EnterLucene.xaml 的交互逻辑
    /// </summary>
    public partial class EnterLucene  //: Window
    {
       
        public EnterLucene()
        {
            InitializeComponent();
        }
   

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(KeyTxt.Text.ToString().Trim()))
            {
                NotifyHelper.Info("请输入密钥");
                return;
            }
            Config.ConfigModel.Token = KeyTxt.Text.ToString().Trim();
             var checkResult = Utll.CheckLucense();
            if(checkResult == true)
            {
                //授权成功
                Config.SaveConfig();
                DialogResult = true;
                this.Close();
            }
            else
            {
                NotifyHelper.Info("密钥错误！");
            }
        }



    }
}
