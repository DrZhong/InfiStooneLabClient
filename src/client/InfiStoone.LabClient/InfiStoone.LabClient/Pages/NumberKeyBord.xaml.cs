using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
using InfiStoone.LabClient.Annotations;

namespace InfiStoone.LabClient.Pages
{
    /// <summary>
    /// EnglishKeyBord.xaml 的交互逻辑
    /// </summary>
    public partial class NumberKeyBord : INotifyPropertyChanged
    {
        public NumberKeyBord(TextBox FocusTextBox):this()
        {
            this.FocusTextBox = FocusTextBox;
        }

        public bool ShowPoint { get; set; } = true;

        public NumberKeyBord()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        /// <summary>
        /// 焦点选中的文本框
        /// </summary>
        public System.Windows.Controls.TextBox FocusTextBox { get; set; }
        public bool NotifyEveryCharacter { get; set; } = false;

        public string Value { get; set; }


        public   event Action<string> PrivateKeybordChanged;

        public static event Action<string> KeybordChanged;

        public   void OnKeybordChanged(string value)
        {
            KeybordChanged?.Invoke(value);
            PrivateKeybordChanged?.Invoke(value);
        }

        private void EventSetter_OnHandler(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null)
            {
                return;
            }
 
            if (btn.Content?.ToString() == "← 删除")
            {
                if (!string.IsNullOrEmpty(Value))
                {
                    this.Value = this.Value.Substring(0, this.Value.Length - 1);
                    if (this.FocusTextBox != null)
                    {
                        this.FocusTextBox.Text = Value;
                    }
                    if (NotifyEveryCharacter)
                    {
                        OnKeybordChanged(Value);
                        Messenger.Default.Send(Value, MessengerToken.KeyBoardWriteComplete);
                    }
                } 
                return;
            }
            else if (btn.Content?.ToString() == "x 清空")
            {
                this.Value = "";
                if (this.FocusTextBox != null)
                {
                    this.FocusTextBox.Text = Value;
                }
                if (NotifyEveryCharacter)
                {
                    OnKeybordChanged(Value);
                    Messenger.Default.Send(Value, MessengerToken.KeyBoardWriteComplete);
                } 
                return;
            }
            else if (btn.Content?.ToString() == "✔确定")
            {
                if (this.FocusTextBox != null)
                {
                    
                }
                OnKeybordChanged("\r");
                Messenger.Default.Send(Value, MessengerToken.KeyBoardWriteEnter); 
                return;
            }
            string value = btn.Content.ToString();
             

            this.Value += value;
            if (this.FocusTextBox != null)
            {
                this.FocusTextBox.Text += value;
            }
            if (NotifyEveryCharacter) {
                OnKeybordChanged(Value);
                Messenger.Default.Send(Value, MessengerToken.KeyBoardWriteComplete);
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
