using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using InfiStoone.LabClient.Runtime;
using InfiStoone.LabClient.Runtime.Entity;
using InfiStoone.LabClient.Services;
using InfiStoone.LabClient.Tools;
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
using System.Windows.Shapes;

namespace InfiStoone.LabClient.Pages
{
    /// <summary>
    /// SelectWarehouse.xaml 的交互逻辑
    /// </summary>
    public partial class SelectWarehouse 
    {
        public SelectWarehouse()
        {
            InitializeComponent(); 
            this.Loaded += SelectWarehouse_Loaded;
        }

        private void SelectWarehouse_Loaded(object sender, RoutedEventArgs e)
        {
            var s= SimpleIoc.Default.GetInstance<DataService>();
            var list= s.GetWareHouse();
          
            //this.SelectBtn.SelectedValuePath = "Id";
            this.SelectBtn.DisplayMemberPath = "Name";
            this.SelectBtn.ItemsSource = list;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var value = this.SelectBtn.SelectedValue as WareHouseDto;
            if (value==null)
            {
                NotifyHelper.Error("请选择仓库");
                return;
            }
            //开始选择
            Session.SelectedWareHouse = value;
            Config.ConfigModel.WareHouse = value;
            Config.SaveConfig();

            Messenger.Default.Send(value, MessengerToken.HouseWareSelectChange);
            this.Close();
        }
    }
}
