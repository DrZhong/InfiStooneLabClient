using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using InfiStoone.LabClient.Services;
using InfiStoone.LabClient.ViewModel.Basic;
using InfiStoone.LabClient.ViewModel.Order;
using InfiStoone.LabClient.ViewModel.LoginChild;

namespace InfiStoone.LabClient.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<BarCodeService>();
            SimpleIoc.Default.Register<DataService>();
            SimpleIoc.Default.Register<FingerService>();
            SimpleIoc.Default.Register<MainViewModel>(); 
            SimpleIoc.Default.Register<DataService>();
            SimpleIoc.Default.Register<HomeViewModel>();
            SimpleIoc.Default.Register<StockQueryViewModel>();
            SimpleIoc.Default.Register<StockInViewModel>();
            SimpleIoc.Default.Register<StockOutViewModel>();
            SimpleIoc.Default.Register<StockBackViewModel>();
            SimpleIoc.Default.Register<InputNumberDialogViewModel>();
            SimpleIoc.Default.Register<OrderViewModel>();
            SimpleIoc.Default.Register<MasterOrderViewModel>();
            SimpleIoc.Default.Register<CommonOrderViewModel>();
            SimpleIoc.Default.Register<ScanBarCodeDialogViewModel>();
            SimpleIoc.Default.Register<AddFingerViewModel>();
            SimpleIoc.Default.Register<FingerprintLoginViewModel>();
            SimpleIoc.Default.Register<AddFingerDetailViewModel>();
        }

        public static ViewModelLocator Instance => new Lazy<ViewModelLocator>(() => Application.Current.TryFindResource("Locator") as ViewModelLocator).Value;   
        public MainViewModel Main => SimpleIoc.Default.GetInstance<MainViewModel>();
        public HomeViewModel Home => SimpleIoc.Default.GetInstance<HomeViewModel>();
        public StockQueryViewModel StockQuery   => SimpleIoc.Default.GetInstance<StockQueryViewModel>();
        public StockInViewModel StockIn => SimpleIoc.Default.GetInstance<StockInViewModel>();
        public StockOutViewModel StockOut => SimpleIoc.Default.GetInstance<StockOutViewModel>();
        public StockBackViewModel StockBack => SimpleIoc.Default.GetInstance<StockBackViewModel>();
        public OrderViewModel Order => SimpleIoc.Default.GetInstance<OrderViewModel>();
        public InputNumberDialogViewModel InputNumberDialog => SimpleIoc.Default.GetInstance<InputNumberDialogViewModel>();
        public ScanBarCodeDialogViewModel ScanBarCodeDialog => SimpleIoc.Default.GetInstance<ScanBarCodeDialogViewModel>();

        public MasterOrderViewModel MasterOrder => SimpleIoc.Default.GetInstance<MasterOrderViewModel>();   
        public CommonOrderViewModel CommonOrder => SimpleIoc.Default.GetInstance<CommonOrderViewModel>();

        public AddFingerViewModel AddFinger => SimpleIoc.Default.GetInstance<AddFingerViewModel>();
        public AddFingerDetailViewModel AddFingerDetail => SimpleIoc.Default.GetInstance<AddFingerDetailViewModel>();
        public FingerprintLoginViewModel FingerprintLogin => SimpleIoc.Default.GetInstance<FingerprintLoginViewModel>();
    }
}
