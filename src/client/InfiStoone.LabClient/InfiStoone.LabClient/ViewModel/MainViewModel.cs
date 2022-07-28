using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using InfiStoone.LabClient.Pages.LoginChild;
using InfiStoone.LabClient.Runtime.Entity;
using InfiStoone.LabClient.Tools.Helper;
using InfiStoone.LabClient.Pages;
using InfiStoone.LabClient.Tools;
using InfiStoone.LabClient.Runtime;
using InfiStoone.LabClient.Services;
using System.IO.Ports;

namespace InfiStoone.LabClient.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly FingerService _fingerService;
        private readonly DataService dataService;
     

        private AbpUser user;
        public bool ShowUser { get; set; } = false;
        public AbpUser User { 
            get => user; 
            set  { 
                user = value; 
                if(value != null)
                {
                    this.ShowUser = true;

                    this.CanAddFinger = value.UserName == "admin";
                }
                else
                {
                    this.ShowUser = false;
                    this.CanAddFinger = false;
                }
            } 
        }

        public bool CanAddFinger { get; set; }

        public bool IsDropDownOpen { get; set; }
        public RelayCommand DropDownOpenCmd => new RelayCommand(() => {
            this.IsDropDownOpen = !this.IsDropDownOpen;
        });
        public RelayCommand SwitchUserCmd => new RelayCommand(() => {
            Session.User = null;
            this.User = null;
            NavCtrl.NavToPage("LoginChild.LoginHome");
        });

        public RelayCommand AddFingerCmd => new RelayCommand(() =>
        {
             NavCtrl.NavToPage("AddFinger");
         });


        /// <summary>
        /// 初始化指纹
        /// </summary>
        public RelayCommand InitFingerCmd => new RelayCommand(() =>
        {
            _fingerService.Reset();
            var list= dataService.GetAllUserFinger();
            int n=0;
            foreach (var item in list)
            {
                try
                {
                    var result = _fingerService.Register(item);
                    if (result)
                    {
                        n++;
                    }
                }
                catch (Exception)
                {
                     
                }
            }
            NotifyHelper.Success($"共注册{list.Count}个指纹，成功{n}个！");
        });


        public DateTime Time { get; set; }
        public DateTime Today { get; set; } = DateTime.Now;
        public static Dictionary<string, object> UserCtrl { get; set; }
        public MainViewModel(FingerService fingerService,
            DataService dataService,
            BarCodeService barCodeService)
        {
            UserCtrl = new Dictionary<string, object>();
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    this.Time = DateTime.Now;
                    Thread.Sleep(1000);
                }
            });

            Messenger.Default.Register<string>(this,
                MessengerToken.LoadShowContent, obj =>
                {

                    if (UserCtrl.ContainsKey(obj))
                    {
                        SubContent = UserCtrl[obj];
                    }
                    else
                    {
                        var ctl = AssemblyHelper.CreateInternalInstance(obj);
                        UserCtrl.Add(obj, ctl);
                        SubContent = ctl;
                    }
                });
            UserCtrl.Add("Pages.LoginChild.LoginHome", new LoginHome());
            this.SubContent = UserCtrl["Pages.LoginChild.LoginHome"];


            Messenger.Default.Register<WareHouseDto>(this, MessengerToken.HouseWareSelectChange, obj =>
            {
                //说明用户选择了仓库
                this.CurrentWarehouse = obj;
            });
            _fingerService = fingerService;
            _fingerService.Init();
            this.dataService = dataService;
            barCodeService.Init();
        }

        
 

        public WareHouseDto CurrentWarehouse { get; set; }

        public RelayCommand SelectWarehouseCmd => new RelayCommand(() =>
         {
             //if (this.User == null)
             //{
             //    NotifyHelper.Warn("当前用户已经登陆到系统，无法切换仓库");
             //    return;
             //}
             new SelectWarehouse().ShowDialog();
         });

        public RelayCommand GoLoginHomeCmd => new RelayCommand(() =>
        {
            NavCtrl.NavToPage("LoginChild.LoginHome");
        });

        /// <summary>
        /// 子页面
        /// </summary>  
        public object SubContent { get; set; }

        /// <summary>
        /// 是否显示返回首页按钮
        /// </summary>
        public bool ShowBackHome { get; set; } = false; 
    }
}
