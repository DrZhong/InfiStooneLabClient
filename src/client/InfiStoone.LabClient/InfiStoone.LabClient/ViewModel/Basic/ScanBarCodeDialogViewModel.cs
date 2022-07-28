using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using InfiStoone.LabClient.Pages;
using InfiStoone.LabClient.Runtime.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiStoone.LabClient.ViewModel.Basic
{
    public class ScanBarCodeDialogViewModel : ViewModelBase, IDialogResultable<OutOrderStockOutInputItem>
    {

        public bool ShowAccount { get; set; } //= false;

        public OutOrderStockOutInputItem Result { get; set; }
        public Action CloseAction { get; set; }

        public RelayCommand OkCmd => new RelayCommand(() =>
        {
            CloseAction?.Invoke();
        });

        /// <summary>
        /// 焦点选中的 文本框
        /// </summary>
        public System.Windows.Controls.TextBox FocusTextBox { get; set; }

        public RelayCommand<System.Windows.RoutedEventArgs> GotFocusCmd => new RelayCommand<System.Windows.RoutedEventArgs>(e =>
         { 
             //this.FocusTextBox=e.Source as System.Windows.Controls.TextBox;
             //if (this._numberKeyBord != null)
             //{
             //    _numberKeyBord.Value=this.FocusTextBox.Text;
             //    _numberKeyBord.FocusTextBox = this.FocusTextBox;
             //}
            
         });

        Sprite sp;
        public RelayCommand<bool> OpenKeyBord => new RelayCommand<bool>((bol) =>
        {
            if (sp == null)
            {

                sp = Sprite.Show(GetNumberKeyBord());
                sp.Focusable = false;
                sp.ShowActivated = false;  
            }
            else
            {
                //判断有没有被关
                if (!sp.IsVisible)
                {
                    sp = Sprite.Show(GetNumberKeyBord());
                    sp.Focusable = false; 
                    sp.ShowActivated = false;
                }
            } 
        });

        Sprite sp2;
        public RelayCommand<bool> OpenKeyBord2 => new RelayCommand<bool>((bol) =>
        {
            if (sp2 == null)
            {

                sp2 = Sprite.Show(GetEnglishKeyBord()); 
            }
            else
            {
                //判断有没有被关
                if (!sp2.IsVisible)
                {
                    sp2 = Sprite.Show(GetEnglishKeyBord()); 
                }
            }
        });
        private EnglishKeyBord GetEnglishKeyBord()
        {
            var obj= new EnglishKeyBord();
            obj.NotifyEveryCharacter = true;
            obj.KeybordChanged += Obj_KeybordChanged1;
            return obj; 
        }

        private void Obj_KeybordChanged1(string obj)
        {
            if (string.IsNullOrEmpty(obj))
            {
                this.Result.BarCode = "";
                return;
            }
            if (obj == "\r")
            {
                //说明按了回车
                sp2.Close();
                return;
            } 
            this.Result.BarCode = obj.Trim();
        }

        private NumberKeyBord _numberKeyBord;
        private NumberKeyBord GetNumberKeyBord()
        {
            _numberKeyBord = new NumberKeyBord();
            //_numberKeyBord.FocusTextBox = this.FocusTextBox;
            _numberKeyBord.Focusable = false;
            _numberKeyBord.NotifyEveryCharacter = true;
            _numberKeyBord.Value = this.Result.Account.ToString();
            _numberKeyBord.PrivateKeybordChanged += Obj_KeybordChanged;
            return _numberKeyBord;
        }

        private void Obj_KeybordChanged(string obj)
        {

            if (string.IsNullOrEmpty(obj))
            {
                return;
            }
            if (obj == "\r")
            {
                //说明按了回车
                sp.Close();
                return;
            }
            if (obj.Trim().Length >= 8)
            {
                return;
            }
             this.Result.Account = Convert.ToInt32(obj.Trim());
        }

        public RelayCommand CloseCmd => new RelayCommand(() =>
        {
            Result = null;
            CloseAction?.Invoke();
        });
    }
}
