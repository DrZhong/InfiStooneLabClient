using Abp.Runtime.Security;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using InfiStoone.LabClient.Services;
using InfiStoone.LabClient.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace InfiStoone.LabClient.ViewModel.LoginChild
{
    public class FingerprintLoginViewModel : ViewModelBase
    {
        private readonly FingerService fingerService;
        private readonly DataService dataService;

        public FingerprintLoginViewModel(FingerService fingerService, DataService dataService)
        {
            this.fingerService = fingerService;

            FingerService.OnDeviceConnected += FingerService_OnDeviceConnected;
            FingerService.OnDeviceOpened += FingerService_OnDeviceOpened;
            FingerService.OnCaptured += FingerService_OnCaptured;
            this.IsDevConnected = this.fingerService.IsDevConnected;
            this.IsDevOpend = this.fingerService.IsDevOpend;
            this.dataService = dataService;
        }

        private void FingerService_OnCaptured(FingerService obj)
        {
            if (!ShowLogin)
            {
                return;
            }
            MemoryStream msFP = new MemoryStream();
            BitmapFormat.GetBitmap(obj.mfpImg, obj.mfpWidth, obj.mfpHeight, ref msFP);
            this.PicFp = BitmapToBitmapImage(new Bitmap(msFP));

            //this.PicFp = ByteArrayToBitmapImage(obj.mfpImg);
            MemoryStream msFV = new MemoryStream();
            BitmapFormat.GetBitmap(obj.mfvImg, obj.mfvWidth, obj.mfvHeight, ref msFV);
            this.PicFv = BitmapToBitmapImage(new Bitmap(msFV));


            var uid= obj.DBHybridIdentify();
            if (uid > -1)
            {
                //说明招到用户了，否则没找到
                //去登陆

                var uf=  DataService.CachedUserFinger.FirstOrDefault(w => w.Id == uid);
                if (uf == null)
                {
                    NotifyHelper.Warn("当前指纹还未写入数据库！！");
                    return;
                }

                dataService.FingerLogin(SimpleStringCipher.Instance.Encrypt(uf.UserId+""));
                NotifyHelper.Success("登陆成功！");
                //登陆成功
                Application.Current.Dispatcher.Invoke(new Action(() => {
                    NavCtrl.NavToPage("Home");
                }));
            }
            else
            {
                NotifyHelper.Warn("当前指纹未注册！");
            }
           
        
        }

        private BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            BitmapImage bitmapImage = new BitmapImage();
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
               // bitmap.Save(ms, bitmap.RawFormat);
                bitmapImage.BeginInit();

                bitmap.Save(ms, ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);  


                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }
            return bitmapImage;
        }
         

     

        public BitmapImage PicFp { get; set; }  

        public BitmapImage PicFv { get; set; }

        public bool IsDevConnected { get; set; }

        public bool IsDevOpend { get; set; }
        private void FingerService_OnDeviceOpened(bool obj)
        {
           this.IsDevOpend=obj;
        }

        private void FingerService_OnDeviceConnected(bool obj)
        {
           this.IsDevConnected=obj;
        }

        public RelayCommand OpenDevCmd => new RelayCommand(() =>
         {
             fingerService.Open();
         });

        //PageInitCmd
        public bool ShowLogin { get; set; }
        public RelayCommand PageInitCmd => new RelayCommand(() =>
        {
            this.ShowLogin = true;
            this.PicFp = new BitmapImage(new Uri("/Resources/Images/1.png", UriKind.RelativeOrAbsolute));
            this.PicFv = new BitmapImage(new Uri("/Resources/Images/2.png", UriKind.RelativeOrAbsolute));
        });

        //PageOutCmd
        public RelayCommand PageOutCmd => new RelayCommand(() =>
        {
            this.ShowLogin = false;
            this.PicFp = null;
            this.PicFv = null;
        });
    }
}
