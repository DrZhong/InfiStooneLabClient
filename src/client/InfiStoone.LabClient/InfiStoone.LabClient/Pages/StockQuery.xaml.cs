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

namespace InfiStoone.LabClient.Pages
{
    /// <summary>
    /// StockQuery.xaml 的交互逻辑
    /// </summary>
    public partial class StockQuery : UserControl
    {
        public StockQuery()
        {
            InitializeComponent();
            Task.Factory.StartNew(() => {
                Thread.Sleep(300);
                this.Dispatcher.Invoke(() => {
                    this.SearchInput.Focus();
                });
            });
        }  

        private void SearchInput_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                DrawerBottom.IsOpen = false;
            }
        }

        private void KeyBord_Click(object sender, RoutedEventArgs e)
        {
            SearchInput.Focus();
            DrawerBottom.IsOpen = !DrawerBottom.IsOpen;
        }
    }
}
