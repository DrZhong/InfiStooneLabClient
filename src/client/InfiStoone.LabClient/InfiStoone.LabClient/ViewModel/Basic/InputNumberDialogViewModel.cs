using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using InfiStoone.LabClient.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiStoone.LabClient.ViewModel.Basic
{
    public class InputNumberDialogViewModel : ViewModelBase, IDialogResultable<decimal>
    {
        private decimal _result;    

        public string Title { get; set; }

        public decimal Result
        {
            get => _result;
#if NET40
            set => Set(nameof(Result), ref _result, value);
#else
            set => Set(ref _result, value);
#endif  
        }
        public Action CloseAction { get; set; }

        public RelayCommand OkCmd => new RelayCommand(() =>
         {
             CloseAction?.Invoke();
         });

        Sprite sp;
        public RelayCommand<bool> OpenKeyBord =>new RelayCommand<bool>((bol) =>
        {
            if (sp == null)
            {
              
                sp = Sprite.Show(GetNumberKeyBord());
                
            }
            else
            {
                //判断有没有被关
                if (!sp.IsVisible)
                {
                    sp = Sprite.Show(GetNumberKeyBord());
                }
            }

        });

        private NumberKeyBord GetNumberKeyBord()
        {
            var obj = new NumberKeyBord();
            obj.NotifyEveryCharacter = true;
            obj.Value=this.Result.ToString();
            obj.PrivateKeybordChanged += Obj_KeybordChanged;
            return obj;
        }

        private   void Obj_KeybordChanged(string obj)
        {

            if (string.IsNullOrEmpty(obj))
            {
                return;
            }
            if(obj == "\r")
            {
                //说明按了回车
                sp.Close();
                return;
            }
            if (obj.Trim().Length >= 12)
            {
                return;
            }
            this.Result = Convert.ToDecimal(obj.Trim());
        }

        public RelayCommand CloseCmd => new RelayCommand(() =>
        {
            Result = 0;
            CloseAction?.Invoke();
        });
    }
}
