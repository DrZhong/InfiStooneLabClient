using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using InfiStoone.LabClient.Pages.Basic;
using InfiStoone.LabClient.Runtime.Entity;
using InfiStoone.LabClient.Services;
using InfiStoone.LabClient.Tools;
using InfiStoone.LabClient.ViewModel.Basic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace InfiStoone.LabClient.ViewModel.Order
{
    public class CommonOrderViewModel : ViewModelBase, IDialogResultable<int>
    {
        private readonly DataService dataService;

        public CommonOrderViewModel(DataService dataService)
        {
            this.dataService = dataService;
            var list = new List<OutOrderStockOutInputItem>(); 
            this.PrepareList=new ObservableCollection<OutOrderStockOutInputItem>(list);
        }


        public RelayCommand<OutOrderStockOutInputItem> RemoveItemCmd => new RelayCommand<OutOrderStockOutInputItem>(item =>
         {
             this.PrepareList.Remove(item); 
         });
            
        public OutOrder order { get; set; }
        public ObservableCollection<OutOrderStockOutInputItem> PrepareList { get; set; }
        public ObservableCollection<OutOrderMasterItem> DataList { get; set; } = new ObservableCollection<OutOrderMasterItem>();
        public int Result { get; set; }
        public Action CloseAction { get; set; }


        /// <summary>
        /// 出库
        /// </summary>
        public RelayCommand OrderStockOutCmd => new RelayCommand(() =>
        {
            var total = this.DataList.ToList().Sum(w => w.StockoutAccount);
            //判断数量是否一直
            if (this.PrepareList.ToList().Sum(w=>w.Account) !=total )
            {
                //数量不等
                NotifyHelper.Error($"你出库的数量和出库单的出库数量不等,请出库 {total} 瓶！","错误");
                return;
            }


            MessageHelper.Confirm("请核对出库试剂是否从出库单所示的位置取得，如果不是，请勿点击确认按钮！", res => {
                if (res)
                {
                   
                    this.dataService.OrderStockOut(this.order.Id,this.PrepareList.ToList());
                    this.PrepareList.Clear();
                    Result = 1;
                    NotifyHelper.Success("出库成功");
                    CloseAction?.Invoke();
                }
            }, "你确定要出库吗？");
        });

        public RelayCommand ScanBarCodeCmd => new RelayCommand(() =>
        {
            //扫描条码去咯
            Dialog.Show<ScanBarCodeDialog>()
        .Initialize<ScanBarCodeDialogViewModel>(vm =>
        {
            vm.Result = new OutOrderStockOutInputItem();
        })
         .GetResultAsync<OutOrderStockOutInputItem>().ContinueWith(str =>
         {
             if (str.Result!=null && str.Result.Account>0)
             {
                 //读取试剂基本信息
                 NormalReagentStockListDto entity;
                 try
                 {
                       entity = dataService.GetNormalReagentStockByCode(str.Result.BarCode);
                 }
                 catch (Exception ex)
                 {
                     NotifyHelper.Error(ex.Message);
                     return;
                 }
                 //从列表中找
                 var item1= this.DataList.FirstOrDefault(w => w.LocationId == entity.LocationId
                 && w.ReagentId==entity.ReagentId);
                 if (item1 == null)
                 {
                     NotifyHelper.Warn($"当前扫描的试剂不属于订单项的位置。当前试剂的位置是：{entity.LocationName}");
                     return;
                 }
                 var finaCount = item1.ScanedAccount + str.Result.Account;
                 if (finaCount > item1.StockoutAccount)
                 {
                     NotifyHelper.Warn($"此试剂最多可以出库：{item1.StockoutAccount} 瓶！");
                     return;
                 }
                 item1.ScanedAccount += str.Result.Account;

                 System.Windows.Application.Current.Dispatcher.Invoke(() =>
                 {
                     var item=this.PrepareList.FirstOrDefault(w=>w.BarCode==str.Result.BarCode);
                     if (item != null)
                     {
                         item.Account+=str.Result.Account;
                     }
                     else
                     {
                         this.PrepareList.Add(str.Result);
                     }
                     
                 });
                
             }
         });
        });


        public RelayCommand CloseCmd => new RelayCommand(() =>
        {
            Result = 0;
            CloseAction?.Invoke();
        });
    }
}
