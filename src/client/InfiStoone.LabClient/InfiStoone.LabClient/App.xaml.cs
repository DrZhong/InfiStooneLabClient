using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Abp.Runtime.Security;
using GalaSoft.MvvmLight.Ioc;
using InfiStoone.LabClient.Runtime;
using InfiStoone.LabClient.Services;
using InfiStoone.LabClient.Tools;

namespace InfiStoone.LabClient
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            SimpleStringCipher.DefaultPassPhrase = MessengerToken.DefaultPassPhrase;
            Config.BuildItems();
            base.OnStartup(e);
            //UI线程未捕获异常处理事件
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;


            //非UI线程未捕获异常处理事件
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                SimpleIoc.Default.GetInstance<FingerService>().Close();
            }
            catch (Exception)
            {
                 
            }

            base.OnExit(e);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogHelper.Error(e.ToString());
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            LogHelper.Error(e.ToString());
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            LogHelper.Error(e.Exception.Message,e.Exception);
            if (e.Exception is UserFriendException exception)
            {
                if (string.IsNullOrEmpty(exception.Detail))
                {
                    //如果有详情
                    NotifyHelper.Error(exception.Message);
                }
                else
                {
                    NotifyHelper.Error(exception.Detail, exception.Message);
                }
              
            }
            else if (e.Exception.InnerException is UserFriendException exception2)
            {
                if (string.IsNullOrEmpty(exception2.Detail))
                {
                    //如果有详情
                    NotifyHelper.Error(exception2.Message);
                }
                else
                {
                    NotifyHelper.Error(exception2.Detail, exception2.Message);
                }
            }
            else
            {
                if (e.Exception.Message != "调用的目标发生了异常。")
                {
                    NotifyHelper.Error(e.Exception.Message);
                } 
            } 
        }
    }
}
