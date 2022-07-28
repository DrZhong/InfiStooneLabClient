using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using HandyControl.Tools.Extension; 
using InfiStoone.LabClient.Pages.OrderChild;
using InfiStoone.LabClient.Runtime.Entity;
using InfiStoone.LabClient.Services;
using InfiStoone.LabClient.Tools;
using InfiStoone.LabClient.ViewModel.Order;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiStoone.LabClient.ViewModel
{
    public class OrderViewModel : ViewModelBase
    {

        private readonly DataService dataService; 

        public OrderViewModel(DataService dataService)
        {
            this.dataService = dataService;
           
            
        }
   

        public RelayCommand GoHomeCmd => new RelayCommand(() =>
        {
            NavCtrl.NavToPage("Home");
        });
        public ObservableCollection<OutOrder> DataList { get; set; } = new ObservableCollection<OutOrder>();
        public RelayCommand SearchCmd => new RelayCommand(() =>
        {
            Search();
        });

        /// <summary>
        /// 过滤项
        /// </summary>
        public string SearchFilter { get; set; }

        public RelayCommand PageInitCmd => new RelayCommand(() =>
        {
            Task.Factory.StartNew(() => {
                Search();
            });
            BarCodeService.OnDataReceived += BarCodeService_OnDataReceived;
        });

        private void BarCodeService_OnDataReceived(string obj)
        {
            this.SearchFilter = obj.Trim();
            this.Search();
        }

        public RelayCommand PageOutCmd => new RelayCommand(() =>
        {
            BarCodeService.OnDataReceived -= BarCodeService_OnDataReceived;
        });

        private void Search()
        {
            List<OutOrder> list=new List<OutOrder>();
            if (!string.IsNullOrEmpty(SearchFilter))
            {
                try
                {
                    list.Add(this.dataService.GetOrderById(SearchFilter));
                }
                catch (Exception exception)
                {
                    NotifyHelper.Error(exception.Message);
                }
            }
            else
            {
               list = this.dataService.GetMyOrder(); 
            }

            System.Windows.Application.Current.Dispatcher.Invoke(() => {
                DataList = new ObservableCollection<OutOrder>(list);
            }); 
        }

        public RelayCommand<OutOrder> StockoutCmd => new RelayCommand<OutOrder>(dt0 =>
        {
            if (dt0.IsMaster)
            {
                //Dialog.Show<Pages.Order.MasterOrder>()
                Dialog.Show<MasterOrder>()
                  .Initialize<MasterOrderViewModel>(vm =>
                  {
                      vm.order = dt0;
                      vm.DataList = new ObservableCollection<OutOrderMasterItem>(dt0.OutOrderMasterItems);
                  })
                   .GetResultAsync<int>().ContinueWith(str =>
                   {
                       if (str.Result>0)
                       {

                           if (this.DataList.Count == 1)
                           {
                               //如果只有一条，直接清空
                               App.Current.Dispatcher.Invoke(() => {
                                   this.DataList.Clear();
                               });
                              
                           }
                           else
                           {
                               //可能是多个出库单
                               this.Search();
                           }
                           //
                       } 
                   });
            }
            else
            {
                //普通试剂
                Dialog.Show<CommonOrder>()
                .Initialize<CommonOrderViewModel>(vm =>
                {
                    vm.order = dt0;
                    vm.DataList = new ObservableCollection<OutOrderMasterItem>(dt0.OutOrderMasterItems);
                })
                 .GetResultAsync<int>().ContinueWith(str =>
                 {
                     if (str.Result > 0)
                     {
                         this.Search();
                     }
                 });
            } 

        });
    }
}
