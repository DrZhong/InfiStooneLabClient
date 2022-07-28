using System;
using System.Windows;
using HandyControl.Controls;
using HandyControl.Data;

namespace InfiStoone.LabClient.Tools
{
    public static class NotifyHelper
    {
        public static void Info(string msg)
        {
            Growl.InfoGlobal(new GrowlInfo()
            {
                Message = msg,
                WaitTime = 3
            });

        }

        public static void Warn(string msg)
        {
            //Growl.WarningGlobal(msg);
            Growl.WarningGlobal(new GrowlInfo()
            {
                Message = msg,
                WaitTime = 3
            });
        }





        public static void Error(string msg)
        {
            Growl.InfoGlobal(new GrowlInfo()
            {
                Message = msg,
                WaitTime =3
            });

        }

        public static void Error(string msg,string title)
        {
            HandyControl.Controls.MessageBox.Show(
                     msg,
                     title,
                     MessageBoxButton.OK,
                     MessageBoxImage.Error); 
        }

        public static void Success(string msg)
        {
            //Application.Current.Dispatcher?.Invoke(() => {});
            Growl.SuccessGlobal(msg);
        }
    }

    public static class MessageHelper
    {
        /// <summary>
        /// 警告框
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="title"></param>
        /// <param name="action"></param>
        public static void Confirm(string msg, Action<bool> action, string title = "提示")
        {
            Application.Current.Dispatcher?.Invoke(() => {
                var result = HandyControl.Controls.MessageBox.Show(
                    msg,
                    title,
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                action(result == MessageBoxResult.Yes);
            });

        }
    }
}