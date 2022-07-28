using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using InfiStoone.LabClient.Runtime.Entity;
using InfiStoone.LabClient.Services;
using InfiStoone.LabClient.Tools;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace InfiStoone.LabClient.ViewModel
{
    /// <summary>
    /// 试剂归还 viewmodel
    /// </summary>
    public class StockBackViewModel : ViewModelBase
    {
        private readonly DataService dataService; 

        public StockBackViewModel(DataService dataService)
        {
            this.dataService = dataService;
        }

        public RelayCommand GoHomeCmd => new RelayCommand(() =>
        {
            NavCtrl.NavToPage("Home");
        });


        public RelayCommand ClearListCmd => new RelayCommand(() =>
         {
             //H7TLEGDF8R
             this.Value= null; ;
             this.DataList.Clear();
         });

        public RelayCommand Back2Cmd => new RelayCommand(() => {
            if (!this.HasReagentStock)
            {
                NotifyHelper.Error("至少需要归还一瓶试剂！");
                return;
            }
            MessageHelper.Confirm("你确定要归还此试剂吗？", res =>
            {
                if (res)
                {
                    this.dataService.MasterStocBackV2(this.ReagentStock.Id,this.Weight);
                    NotifyHelper.Success("试剂归还成功！");
                    this.ReagentStock = null;
                    this.SearchMsg = "请持扫码枪扫描试剂瓶条形码，听到滴一声后，确定归还清单中内容与所领试剂是否一致";
                    this.Value = null;
                }
            });
        });

        public decimal Weight { get; set; }

        public RelayCommand BackCmd => new RelayCommand(() => {
            if (!this.DataList.Any())
            {
                NotifyHelper.Error("至少需要归还一瓶试剂！");
                return;
            }
            MessageHelper.Confirm("你确定要归还列表中的所有试剂吗？", res =>
            {
                if (res)
                {
                    this.dataService.MasterStocBack(this.DataList.Select(q => q.Id).ToList());
                    NotifyHelper.Success("试剂归还成功！");
                    this.DataList.Clear();
                    this.Value = null;
                }
            }); 
        });

        public RelayCommand<ReagentStockDto> RmLineCmd => new RelayCommand<ReagentStockDto>(line =>
         {
             this.DataList.Remove(line);    
         });

        public string Value { get; set; }
        public RelayCommand SearchCmd => new RelayCommand(() =>
        {
            if (string.IsNullOrEmpty(Value))
            {
                NotifyHelper.Warn("请输入条码");
                return;
            }
 
            Search();
            //ReagentStock = this.dataService.GetReagentStockbyCode(value);
        });
        public RelayCommand PageInitCmd => new RelayCommand(() =>
        {
            BarCodeService.OnDataReceived += BarCodeService_OnDataReceived;
        });

        private void BarCodeService_OnDataReceived(string obj)
        {
            this.Value = obj.Trim();
            this.Search();
        }

        public RelayCommand PageOutCmd => new RelayCommand(() =>
        {
            BarCodeService.OnDataReceived -= BarCodeService_OnDataReceived;
        });


        public ObservableCollection<ReagentStockDto> DataList { get; set; } = new ObservableCollection<ReagentStockDto>();

        public ReagentStockDto ReagentStock { get; set; }

        public bool HasReagentStock => this.ReagentStock != null;
        public string SearchMsg { get; set; } = "请持扫码枪扫描试剂瓶条形码，听到滴一声后，确定归还清单中内容与所领试剂是否一致";
        private void Search()
        {
            this.ReagentStock = null;
            try
            {
                this.ReagentStock = this.dataService.GetReagentStockBackDetailbyCode(Value);
                this.Weight = this.ReagentStock.Weight;
                this.SearchMsg = "";
            }
            catch (Exception ex)
            {
                this.SearchMsg = ex.Message;
            }
            //DataList.Add(this.dataService.GetReagentStockBackDetailbyCode(Value));

        }
    }
}
