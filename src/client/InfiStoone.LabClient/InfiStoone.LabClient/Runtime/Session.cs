using GalaSoft.MvvmLight.Messaging;
using InfiStoone.LabClient.Runtime.Entity;
using System.Collections.Generic;

namespace InfiStoone.LabClient.Runtime
{
    public static class Session
    {
        private static AbpUser user;

        public static AbpUser User {
            get => user;
            set {
                //说明登陆成功了，发布事件
                user = value;
                if (value != null)
                {
                    Messenger.Default.Send(value, MessengerToken.LoginSuccess);
                } 
            }
        } 


        /// <summary>
        /// 选中的仓库
        /// </summary>
        public static WareHouseDto SelectedWareHouse { get; set; }

        public static List<WarehousePermissionDto> CurrentPermission { get; set; }
    }
}