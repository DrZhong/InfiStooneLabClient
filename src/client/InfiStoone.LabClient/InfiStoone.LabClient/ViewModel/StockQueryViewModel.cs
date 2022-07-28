using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
    public class StockQueryViewModel : ViewModelBase
    {
        private readonly DataService dataService;
        public StockQueryViewModel(DataService dataService )
        {
            this.dataService = dataService;
            DataList = new ObservableCollection<ClientStockDto>(new List<ClientStockDto>());
        
        }

        public RelayCommand GoHomeCmd => new RelayCommand(() =>
         {
             NavCtrl.NavToPage("Home");
         });

        public ObservableCollection<ClientStockDto> DataList { get; set; }

        public RelayCommand<string> SearchCmd => new RelayCommand<string>(value =>
        {
           var list=  this.dataService.GetClientStockDto(value);
            this.DataList = new ObservableCollection<ClientStockDto>(list);
            if (list.Count == 0)
            {
                NotifyHelper.Warn("未找到库存信息");
            }
        });
    }
}
