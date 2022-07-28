using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using InfiStoone.LabClient.Runtime.Entity;
using InfiStoone.LabClient.Services;
using InfiStoone.LabClient.Tools;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace InfiStoone.LabClient.ViewModel
{
    public class AddFingerDetailViewModel : ViewModelBase
    { 
        public static UserDto SelectedUser { get; set; } 
        private readonly DataService _dataService;
        private readonly FingerService _fingerService;

        public AddFingerDetailViewModel(DataService dataService, FingerService fingerService)
        {
            _dataService = dataService;
            _fingerService = fingerService; 
            FingerService.OnDeviceConnected += FingerService_OnDeviceConnected;
            FingerService.OnDeviceOpened += FingerService_OnDeviceOpened;
            FingerService.OnCaptured += FingerService_OnCaptured;
            this.IsDevConnected = this._fingerService.IsDevConnected;
            this.IsDevOpend = this._fingerService.IsDevOpend;
        }
        public RelayCommand OpenDevCmd => new RelayCommand(() =>
        {
            _fingerService.Open();
        });

        private void FingerService_OnDeviceOpened(bool obj)
        {
            this.IsDevOpend=obj;
        }
        private void FingerService_OnDeviceConnected(bool obj)
        {
            this.IsDevConnected = obj;
        }

        public int CapturCount { get; set; }

        /// <summary>
        /// 开始注册
        /// </summary>
        public bool BeginRegister { get; set; }

        public RelayCommand BeginRegisterCmd => new RelayCommand(() =>
        {
            this.ClearFinger();
            this.BeginRegister = true;
            this.CapturCount = 0;
        });


        public RelayCommand BackCmd => new RelayCommand(() =>
        {
            NavCtrl.NavToPage("AddFinger");
        });


        private void FingerService_OnCaptured(FingerService obj)
        {
            if (!PageAlive) return;
            if(!BeginRegister) return; 
            if (this.CapturCount > 2) return; 
            obj.mEnrollIdx=this.CapturCount;
            if (this.CapturCount == 0)
            {
                //第一次
                this.PicFp1 = BitmapToBitmapImage(obj.mfpImg, obj.mfpWidth, obj.mfpHeight);
                this.PicFv1= BitmapToBitmapImage(obj.mfvImg, obj.mfvWidth, obj.mfvHeight);
            }
            else if(this.CapturCount == 1)
            {
                //第二次
                this.PicFp2 = BitmapToBitmapImage(obj.mfpImg, obj.mfpWidth, obj.mfpHeight);
                this.PicFv2 = BitmapToBitmapImage(obj.mfvImg, obj.mfvWidth, obj.mfvHeight);
            }
            else if(this.CapturCount == 2)
            {
                
                //第三次
                this.PicFp3 = BitmapToBitmapImage(obj.mfpImg, obj.mfpWidth, obj.mfpHeight);
                this.PicFv3 = BitmapToBitmapImage(obj.mfvImg, obj.mfvWidth, obj.mfvHeight);
            }
            var bol = obj.Verify();
            if (!bol)
            {
                NotifyHelper.Warn("请按压同一根手指！");
                this.ClearFinger();
                return;
            }
            if(this.CapturCount == 2)
            {
                int fid = _dataService.GetMaxFingerId();
                //获取当前最大的id
                var target = new List<string>() { "", "" , "" , "" , "" , "" };
                obj.Register(fid, target);
                var entity = new UserFingerDto() {
                    Id = fid,
                    UserId=this.User.Id,
                    Data1= target[0],
                    Data2 = target[1],
                    Data3 = target[2],
                    Data4 = target[3],
                    Data5 = target[4],
                    Data6 = target[5],
                };

                DataService.CachedUserFinger.Add(entity);
                _dataService.AddUserFinger(entity);
                NotifyHelper.Warn("注册成功！");
                this.ClearFinger();
                return;
                //最后一次
            }

            this.CapturCount++;
        }


        private BitmapImage BitmapToBitmapImage(byte[] buffer, int nWidth, int nHeight)
        {
            MemoryStream msFP = new MemoryStream();
            BitmapFormat.GetBitmap(buffer, nWidth, nHeight, ref msFP);


            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(msFP);
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



        public bool IsDevConnected { get; set; } 
        public bool IsDevOpend { get; set; }
        public UserDto User { get; set; }
        public RelayCommand PageInitCmd => new RelayCommand(() =>
        {
            this.User = SelectedUser;
            this.PageAlive = true;
        });
        //PageOutCmd
        public RelayCommand PageOutCmd => new RelayCommand(() =>
        {
            this.PageAlive = false;
            this.ClearFinger();
        });

        private void ClearFinger()
        {
            this.CapturCount = 0;
            this.PicFp1 = null;
            this.PicFp2 = null;
            this.PicFp3 = null;

            this.PicFv1 = null;
            this.PicFv2 = null;
            this.PicFv3 = null;
        }

        public bool PageAlive { get; set; }
        public BitmapImage PicFp1 { get; set; }
        public BitmapImage PicFv1 { get; set; }

        public BitmapImage PicFp2 { get; set; } 
        public BitmapImage PicFv2 { get; set; }


        public BitmapImage PicFp3 { get; set; } 
        public BitmapImage PicFv3 { get; set; }
    }
}
