using System;

namespace InfiStoone.LabClient.Tools
{
    public enum LogLevel
    {
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }
    public class LogHelper
    {
        static LogHelper()
        {
            log4net.Config.XmlConfigurator.Configure(); // only config one time
        }

        /// <summary>
        /// 当日志发生的时候
        /// </summary>
        public static event Action<LogLevel, string> LogOccured;
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger("logerror");


        public static void Debug(string msg)
        {
            if (String.IsNullOrEmpty(msg))
            {
                return;
            }
            if (Log.IsDebugEnabled) Log.Debug(msg);

            OnLogOccured(LogLevel.Debug, msg);
        }
        public static void DebugFormat(string msg, params object[] args)
        {
            if (String.IsNullOrEmpty(msg))
            {
                return;
            }
            if (Log.IsDebugEnabled) Log.DebugFormat(msg, args);

            OnLogOccured(LogLevel.Debug, string.Format(msg, args));
        }

        public static void Info(string msg)
        {
            if (Log.IsInfoEnabled)
            {
                Log.Info(msg);
            }
            //FileLogger.Info(s);
            OnLogOccured(LogLevel.Info, msg);

        }

        public static void InfoFormat(string msg, params object[] args)
        {
            if (Log.IsInfoEnabled)
            {
                Log.InfoFormat(msg, args);
            }
            //FileLogger.Info(s);
            OnLogOccured(LogLevel.Info, string.Format(msg, args));

        }

        public static void Error(string s, System.Exception ex)
        {
            if (string.IsNullOrEmpty(s))
            {
                return;
            }
            if (Log.IsErrorEnabled) Log.Error(s, ex);
            OnLogOccured(LogLevel.Error, s);
        }
        public static void Error(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return;
            }
            if (Log.IsErrorEnabled) Log.Error(s);
            OnLogOccured(LogLevel.Error, s);
        }

        public static void WarnFormat(string msg, params object[] args)
        {
            if (string.IsNullOrEmpty(msg))
            {
                return;
            }
            if (Log.IsWarnEnabled) Log.WarnFormat(msg, args);
            OnLogOccured(LogLevel.Warn, msg);
        }

        public static void Warn(string msg)
        {
            if (string.IsNullOrEmpty(msg))
            {
                return;
            }
            if (Log.IsWarnEnabled) Log.Warn(msg);
            OnLogOccured(LogLevel.Warn, msg);
        }

        /// <summary>
        /// 致命的错误
        /// </summary>
        /// <param name="s"></param>
        public static void Fatal(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return;
            }
            if (Log.IsFatalEnabled) Log.Fatal(s);
            OnLogOccured(LogLevel.Fatal, s);
        }

        private static void OnLogOccured(LogLevel logLevel, string msg)
        {
            LogOccured?.Invoke(logLevel, msg);
        }
    }
}