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
using System.Text;
using System.Threading.Tasks;

namespace InfiStoone.LabClient.ViewModel.Order
{
    /// <summary>
    /// 专管出库
    /// </summary>
    public class MasterOrderViewModel : ViewModelBase, IDialogResultable<int>
    {
        private readonly DataService dataService;

        public MasterOrderViewModel(DataService dataService)
        {
            this.dataService = dataService; 
        }

        private int _result;

        public OutOrder order { get; set; } 

        public ObservableCollection<OutOrderMasterItem> DataList { get; set; } = new ObservableCollection<OutOrderMasterItem>();    
        public int Result
        {
            get => _result;
#if NET40
            set => Set(nameof(Result), ref _result, value);
#else
            set => Set(ref _result, value);
#endif  
        }
        public Action CloseAction { get; set; }

        
        /// <summary>
        /// 出库
        /// </summary>
        public RelayCommand OrderStockOutCmd => new RelayCommand(() =>
        {
            if (this.DataList.Any(w => !w.Scaned))
            {
                NotifyHelper.Warn("请先扫描所有出库单的时候确认后再出库！");
                return;
            }

            MessageHelper.Confirm("请核对取出的专管试剂条码是否和列表中的条码完全一致，如果不一致，请勿点击确认按钮！", res => { 
                if (res)
                {
                    Result = 1;
                    this.dataService.OrderStockOut(this.order.Id);
                    NotifyHelper.Success("出库成功");
                    CloseAction?.Invoke();
                } 
            }, "你确定要出库吗？");
        });

        public RelayCommand ScanCodeCmd => new RelayCommand(() => {
            //扫描条码去咯
            Dialog.Show<ScanBarCodeDialog>()
        .Initialize<ScanBarCodeDialogViewModel>(vm =>
        {
            vm.Result = new OutOrderStockOutInputItem();
            vm.ShowAccount = true;
        })
         .GetResultAsync<OutOrderStockOutInputItem>().ContinueWith( str =>
         {
             var result = str.Result;

             if (result != null)
             {
                 var item = this.DataList.FirstOrDefault(w => w.ReagentStockBarCode == result.BarCode);
                 if (item == null)
                 {
                     NotifyHelper.Warn($"当前扫描的试剂不属于订单项的位置。");
                     return;
                 }
                 item.Scaned = true;

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
