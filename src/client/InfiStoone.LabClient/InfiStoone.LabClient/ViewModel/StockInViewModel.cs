using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using InfiStoone.LabClient.Pages.Basic;
using InfiStoone.LabClient.Runtime;
using InfiStoone.LabClient.Runtime.Entity;
using InfiStoone.LabClient.Services;
using InfiStoone.LabClient.Tools;
using InfiStoone.LabClient.ViewModel.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiStoone.LabClient.ViewModel
{
    public class StockInViewModel : ViewModelBase
    {
        private readonly DataService dataService;
        private ReagentStockDto reagentStock;

        public StockInViewModel(DataService dataService)
        {
            this.dataService = dataService;
        }

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

        /// <summary>
        /// 入库
        /// </summary>
        public RelayCommand StockInCmd => new RelayCommand(() =>
        {
            if (reagentStock != null && reagentStock.CanStaockIn)
            {
                if (reagentStock.IsMaster)
                {
                    if (this.Weight == 0)
                    {
                        MessageHelper.Confirm("当前入库的重量为0,你确定要入库吗？", res =>
                        {
                            if (res)
                            {
                                this.dataService.MasterStockIn(this.reagentStock.Id, this.Weight);
                                NotifyHelper.Success("入库成功");
                                this.Search();
                            }
                        });

                    }
                    MessageHelper.Confirm("你确定要入库吗？", res =>
                    {
                        if (res)
                        {
                            this.dataService.MasterStockIn(this.reagentStock.Id, this.Weight);
                            NotifyHelper.Success("入库成功");
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
                          vm.Title = "请输入入库数量";
                          vm.Result = reagentStock.Amount - reagentStock.RealAmount;
                      })
                       .GetResultAsync<decimal>().ContinueWith(str =>
                       {
                           if (str.Result == 0)
                           {
                               //取消
                               NotifyHelper.Info("取消入库");
                           }
                           else if (str.Result < 0)
                           {
                               NotifyHelper.Warn("入库数量不允许小于0！");
                               return;
                           }
                           else
                           {
                               //
                               try
                               {
                                   this.dataService.NormalStockIn(this.reagentStock.Id, (int)str.Result);
                                   NotifyHelper.Success("入库成功");
                                   this.Search();
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
                                   };
                               }
                               catch (Exception ex)
                               {
                                   throw ex;
                               }

                           }
                       });
                }
            }
            else
            {
                NotifyHelper.Warn("请先扫描待入库或者离库的试剂");
            }
        });

        public bool HasReagentStock { get; set; }

        /// <summary>
        /// 普通试剂入库数量
        /// </summary>
        public int Account { get; set; }

        /// <summary>
        /// 专管试剂入库的重量
        /// </summary>
        public decimal Weight { get; set; }
        public string Value { get; set; }
        public RelayCommand SearchCmd => new RelayCommand(() =>
        {
            //this.Value = value;
            Search();
            //ReagentStock = this.dataService.GetReagentStockbyCode(value);
        });

        public string SearchMsg { get; set; } = "请输入或者扫描试剂二维码后，点击界面上的确认入库按钮进行入库";
        private void Search()
        {
            if (string.IsNullOrEmpty(Value))
            {
                NotifyHelper.Info("请输入或者扫描试剂二维码后后在搜索");
                return;
            }
            try
            {
                ReagentStock = this.dataService.GetReagentStockbyCode(Value);
                this.SearchMsg = "";
            }
            catch (Exception ex)
            {
                ReagentStock = null;
                this.SearchMsg = ex.Message;
            }
        }
    }
}
