using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using InfiStoone.LabClient.Runtime;
using InfiStoone.LabClient.Runtime.Entity;
using InfiStoone.LabClient.Services;
using System.Linq;
using InfiStoone.LabClient.Tools;

namespace InfiStoone.LabClient.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly MainViewModel mainViewModel;
        private readonly DataService _dataService;
    
        public HomeViewModel(
            DataService dataService,
            MainViewModel mainViewModel )
        {

            this.mainViewModel = mainViewModel;
            this._dataService = dataService;
            Messenger.Default.Register<AbpUser>(this,
          MessengerToken.LoginSuccess, obj =>
          {

              InitPermission();

          });
            InitPermission(); 
            // Messenger.Default.Send(value, MessengerToken.LoginSuccess);
           
        }

        private void InitPermission()
        {
            var permission = this._dataService.GetWarehousePermission();
            this.CanQuery = permission.Any(w => w.Permission == WarehousePermissionEnum.库存查询);
            this.CanStockin = permission.Any(w => w.Permission == WarehousePermissionEnum.试剂入库);
            this.CanStockout = permission.Any(w => w.Permission == WarehousePermissionEnum.试剂领用);
            this.CanStockback = permission.Any(w => w.Permission == WarehousePermissionEnum.试剂归还);
            this.CanOrder = permission.Any(w => w.Permission == WarehousePermissionEnum.出库单);
        }

        /// <summary>
        /// 能否领用
        /// </summary>
        public bool CanQuery { get; set; }
        /// <summary>
        /// 能否入库
        /// </summary>
        public bool CanStockin { get; set; }
        /// <summary>
        /// 能否出库
        /// </summary>
        public bool CanStockout { get; set; }
        /// <summary>
        /// 能够领用
        /// </summary>
        public bool CanStockback { get; set; }
        /// <summary>
        /// 能够使用出库单
        /// </summary>
        public bool CanOrder { get; set; }


        public RelayCommand<string> GoFunPageCmd => new RelayCommand<string>(page =>
         {
             NavCtrl.NavToPage(page);
         });

        public RelayCommand<EventArgs> LoadedCmd => new RelayCommand<EventArgs>(arg =>
        {
            mainViewModel.User = Session.User;
        });
    }
}