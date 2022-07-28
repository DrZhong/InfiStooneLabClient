using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Data;
using InfiStoone.LabClient.Runtime.Entity;
using InfiStoone.LabClient.Services;
using InfiStoone.LabClient.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiStoone.LabClient.ViewModel
{
    public class AddFingerViewModel : ViewModelBase
    {

        private readonly DataService dataService;

        public AddFingerViewModel(DataService dataService)
        {
            this.dataService = dataService;


        }
        //PageInitCmd
        public RelayCommand PageInitCmd => new RelayCommand(() =>
        {
            Task.Factory.StartNew(() => {
                Search();
            });
        });

        public RelayCommand GoHomeCmd => new RelayCommand(() =>
        {
            NavCtrl.NavToPage("Home");
        });
        public ObservableCollection<UserDto> DataList { get; set; } = new ObservableCollection<UserDto>();
        public RelayCommand SearchCmd => new RelayCommand(() =>
        {
            Search();
        });

        /// <summary>
        /// 过滤项
        /// </summary>
        public string SearchFilter { get; set; }

        //int pageIndex, int pageSize
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public int TotalPage { get; set; } = 1;
        public long Total { get; set; }
        private void Search()
        {
            var result = dataService.GetUser(this.SearchFilter, PageIndex, PageSize);
            this.Total = result.TotalCount;
            this.DataList = new ObservableCollection<UserDto>(result.Items);
            this.TotalPage = (int)(Total / this.PageSize) + 1;
        }

        public RelayCommand<UserDto> StockoutCmd => new RelayCommand<UserDto>(dt0 =>
        {
            AddFingerDetailViewModel.SelectedUser = dt0;
            NavCtrl.NavToPage("AddFingerDetail");
        });

        /// <summary>
        ///     页码改变命令
        /// </summary>
        public RelayCommand<FunctionEventArgs<int>> PageUpdatedCmd => new RelayCommand<FunctionEventArgs<int>>(o =>
        {
            this.PageIndex = o.Info;
            this.Search();
        });
    }
}
