using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents; 
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;
using HandyControl.Tools;
using InfiStoone.LabClient.Annotations;

namespace InfiStoone.LabClient.Pages
{
    /// <summary>
    /// EnglishKeyBord.xaml 的交互逻辑
    /// </summary>
    public partial class EnglishKeyBord : INotifyPropertyChanged
    {  /// <summary>
       /// 查找子控件
       /// </summary>
       /// <typeparam name="T">控件类型</typeparam>
       /// <param name="parent">父控件依赖对象</param>
       /// <param name="lstT">子控件列表</param>
        public static void FindVisualChild<T>(DependencyObject parent, ref List<T> lstT) where T : DependencyObject
        {
            if (parent != null)
            {
                T child = default(T);
                int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < numVisuals; i++)
                {
                    Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                    child = v as T;
                    if (child != null)
                    {
                        lstT.Add(child);
                    }
                    FindVisualChild<T>(v, ref lstT);
                }
            }
        }
     
        public EnglishKeyBord()
        {
            InitializeComponent();
            this.DataContext = this;

            //把所有button的Focusable设置为false

            var list = new List<Button>();
            FindVisualChild<Button>(this, ref list);
            foreach (var item in list)
            {
                item.Focusable = false;
            }

        }

        public bool NotifyEveryCharacter { get; set; } = false;

        public string Value { get; set; }

        public ObservableCollection<string> FirstRow { get; set; } = new ObservableCollection<string>(new List<string>()
        {
            "q","w","e","r","t","y","u","i","o","p"
        });
        public ObservableCollection<string> SecondRow { get; set; } = new ObservableCollection<string>(new List<string>()
        {
            "a","s","d","f","g","h","j","k","l",";","'"
        });

        public ObservableCollection<string> ThirdRow { get; set; } = new ObservableCollection<string>(new List<string>()
        {
            "z","x","c","v","b","n","m",",",".","/","?"
        });

        public  event Action<string> KeybordChanged;

        public  void OnKeybordChanged(string value)
        {
            KeybordChanged?.Invoke(value);
        }

        private void EventSetter_OnHandler(object sender, RoutedEventArgs e)
        {
            var btn = sender as System.Windows.Controls.Button;
            if (btn == null)
            {
                return;
            }

            if (btn.Content?.ToString() == "大写")
            {
                btn.Content = "小写";
                this.FirstRow = new ObservableCollection<string>(this.FirstRow.Select(w => w.ToUpper()).ToList());
                this.SecondRow = new ObservableCollection<string>(this.SecondRow.Select(w => w.ToUpper()).ToList());
                this.ThirdRow = new ObservableCollection<string>(this.ThirdRow.Select(w => w.ToUpper()).ToList());
                return;
            }
            else if (btn.Content?.ToString() == "小写")
            {
                btn.Content = "大写";
                this.FirstRow = new ObservableCollection<string>(this.FirstRow.Select(w => w.ToLower()).ToList());
                this.SecondRow = new ObservableCollection<string>(this.SecondRow.Select(w => w.ToLower()).ToList());
                this.ThirdRow = new ObservableCollection<string>(this.ThirdRow.Select(w => w.ToLower()).ToList());
                return;
            }
            else if (btn.Content?.ToString() == "← 删除")
            {
                Task.Factory.StartNew(() => { 
                    System.Windows.Forms.SendKeys.SendWait("{BACKSPACE}");
                });

                if (!string.IsNullOrEmpty(Value))
                {
                   this.Value = this.Value.Substring(0, this.Value.Length - 1);
                    if (NotifyEveryCharacter)
                    {
                        OnKeybordChanged(Value); 
                    }
                }
                return;
            }
            else if (btn.Content?.ToString() == "x 清空")
            {
                Task.Factory.StartNew(() => { 
                    System.Windows.Forms.SendKeys.SendWait("{BACKSPACE 50}");
                });
                this.Value = "";
                if (NotifyEveryCharacter)
                {
                    OnKeybordChanged(Value); 
                }
                return;
            }
            else if (btn.Content?.ToString() == "✔确定")
            {
                Task.Factory.StartNew(() => { 
                    System.Windows.Forms.SendKeys.SendWait("{ENTER}");
                });
                OnKeybordChanged("\r"); 
                return;
            }
             
            string value = btn.Content.ToString();
            Task.Factory.StartNew(() => {
                //Thread.Sleep(200);
                System.Windows.Forms.SendKeys.SendWait(value);
            });
            this.Value += btn.Content;
            if (NotifyEveryCharacter)
            {
                OnKeybordChanged(Value); 
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
