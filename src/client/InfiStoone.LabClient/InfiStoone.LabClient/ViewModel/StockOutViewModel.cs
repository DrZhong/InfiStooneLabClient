using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using InfiStoone.LabClient.Pages.Basic;
using InfiStoone.LabClient.Runtime.Entity;
using InfiStoone.LabClient.Services;
using InfiStoone.LabClient.Tools;
using InfiStoone.LabClient.ViewModel.Basic;
using System; 
using HandyControl.Tools.Extension;
using System.Collections.ObjectModel;
using System.Linq;
using InfiStoone.LabClient.Runtime;

namespace InfiStoone.LabClient.ViewModel
{
    public class StockOutViewModel : ViewModelBase
    {
        private readonly DataService dataService;
        private ReagentStockDto reagentStock;

        public StockOutViewModel(DataService dataService)
        {
            this.dataService = dataService;
        }

        public RelayCommand GoHomeCmd => new RelayCommand(() =>
        {
            NavCtrl.NavToPage("Home");
        });

        public ReagentStockDto ReagentStock
        {
            get => reagentStock;
            set
            {
                this.HasReagentStock = (value != null);
                reagentStock = value;
            }
        }
 

        public bool HasReagentStock { get; set; }

        public RelayCommand ClearListCmd => new RelayCommand(() => { 
            this.DataList.Clear();
        });

        public RelayCommand<int> EditSelectNumCmd => new RelayCommand<int>(record => { 

        });

        public RelayCommand<int> DeleteSelectedCmd => new RelayCommand<int>(record => {

        });

        public RelayCommand<ClientStockDto> StockoutCmd => new RelayCommand<ClientStockDto>(dt0 =>
         {
             if (dt0.IsMaster)
             {
                 //  Dialog.Show<InputNumberDialog>()
                 //.Initialize<InputNumberDialogViewModel>(vm =>
                 //{
                 //    vm.Title = "请输入此试剂的重量";
                 //    vm.Result = 1;
                 //})
                 // .GetResultAsync<decimal>().ContinueWith(str =>
                 // {
                 //     if (str.Result == 0)
                 //     {
                 //             //取消
                 //             NotifyHelper.Info("取消领用");
                 //     }
                 //     else
                 //     {
                 //         this.dataService.MasterStockout(dt0.BarCode, dt0.PrepareToStockOutNum, str.Result);
                 //         //this.dataService.MasterStockout(dt0.BarCode, str.Result);
                 //         NotifyHelper.Success("领用成功");
                 //         this.Search();
                 //     }
                 // });
                 MessageHelper.Confirm("你确定要领用此试剂吗？", res =>
                 {
                     if (res)
                     {
                         this.dataService.MasterStockout(dt0.BarCode, dt0.PrepareToStockOutNum,0);
                         NotifyHelper.Success("领用成功");
                         this.Value = null;
                         this.Search();
                     }
                 });
             }
             else
             {
                 //普通试剂
                 Dialog.Show<InputNumberDialog>() 
                   .Initialize<InputNumberDialogViewModel>(vm =>
                   {
                       vm.Title = "请输入出库数量";
                       vm.Result = 1;
                   })
                    .GetResultAsync<decimal>().ContinueWith(  str =>
                    {
                        if (str.Result == default(decimal))
                        {
                            //取消
                            NotifyHelper.Info("取消领用");
                        } else if (str.Result<0) 
                        {
                            NotifyHelper.Warn("领用数量不能小于0！");
                            return;
                        }
                        else
                        {
                            //
                            this.dataService.MasterStockout(dt0.BarCode, (int)str.Result, 0);
                            NotifyHelper.Success("领用成功");
                            this.Value = null;
                            this.Search();
                        }
                    });
             } 
         });

        public string Value { get; set; }
        public RelayCommand  SearchCmd => new RelayCommand (() =>
        {
            if (string.IsNullOrEmpty(Value))
            {
                NotifyHelper.Info("请输入条码");
                return;
            }
            var entity = this.DataList.FirstOrDefault(w => w.BarCode == this.Value);
            if (entity!=null){
                //如果已经存在了
                if(entity.Num == entity.PrepareToStockOutNum)
                {
                    NotifyHelper.Warn($"此试剂剩余库存数只剩下{entity.Num}瓶，无法继续出库");
                    return;
                }
                entity.PrepareToStockOutNum++; 
                DataList  = new ObservableCollection<ClientStockDto>(DataList.ToList());
                return;
            }

            Search();
            if (DataList.Count == 0)
            {
                NotifyHelper.Info("未找到条码对应的试剂");
            }
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


        public ObservableCollection<ClientStockDto> DataList { get; set; } = new ObservableCollection<ClientStockDto>();

        private void Search()
        {
            App.Current.Dispatcher.Invoke(() => {
                if (string.IsNullOrEmpty(this.Value))
                {
                    DataList.Clear();
                    return;
                }
                try
                {
                    DataList = new ObservableCollection<ClientStockDto>(this.dataService.GetClientStockByCodeDto(Value));

                }
                catch (UserFriendException exception)
                {
                    if (string.IsNullOrEmpty(exception.Detail))
                    {
                        //如果有详情
                        NotifyHelper.Error(exception.Message);
                    }
                    else
                    {
                        NotifyHelper.Error(exception.Detail, exception.Message);
                    }
                }
                catch(Exception e) {
                    NotifyHelper.Error(e.Message);
                }
            });
          
        }
    }
}
