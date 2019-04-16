using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Collections.Generic;

using log4net;
using log4net.Config;

namespace Contec.Framework.Logging
{
    public class Log4NetLogService : ILogService
    {
        public static string InitializationPath;

        public Log4NetLogService(string log4NetFilePath)
        {
            EnsureConfigured(log4NetFilePath);
        }

        public bool IsInitialized => Interlocked.Read(ref _isInitialized) != 0;

        public void EnsureConfigured(string log4NetFilePath)
        {
            // If we are already initialized
            if (Interlocked.Read(ref _isInitialized) != 0) return;

            // Acquire the appropriate lock
            lock (InitializationLock)
            {
                // Once we get through the lock, did a previous process already initialize us?
                // If so, get outta here
                if (Interlocked.Read(ref _isInitialized) != 0) return;

                InitializationPath = log4NetFilePath;
                try
                {
                    if (!string.IsNullOrWhiteSpace(log4NetFilePath))
                    {
                        if (!File.Exists(log4NetFilePath))
                        {
                            // If the file doesnt exist, attempt to prepend the current path .... and retry
                            log4NetFilePath = PrependCurrentPathForMissingConfigurationFile(log4NetFilePath);
                        }

                        if (File.Exists(log4NetFilePath))
                        {
                            var configFile = new FileInfo(log4NetFilePath);
                            XmlConfigurator.ConfigureAndWatch(configFile);
                            return;
                        }
                    }

                    // Lets go to our default, embedded configuration if we cannot find the appropriate configuration
                    ConfigureLoggingToUseDefaultConfiguration(log4NetFilePath ?? string.Empty);
                    ApplicationName(string.Empty);
                }
                finally
                {
                    // This tells every other process that we are already initialized    
                    Interlocked.Increment(ref _isInitialized);
                }
            }
        }

        private static string PrependCurrentPathForMissingConfigurationFile(string providedFileName)
        {
            try
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                var directoryInfo = new FileInfo(path).Directory;
                if (directoryInfo != null)
                {
                    var root = directoryInfo.FullName;
                    return Path.Combine(root, providedFileName);
                }
            }
            catch { }

            return providedFileName;
        }

        private static void ConfigureLoggingToUseDefaultConfiguration(string log4NetFilePath)
        {
            XmlConfigurator.Configure();
        }

        public void ApplicationName(string appName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(appName))
                {
                    GlobalContext.Properties["appname"] = appName;
                    return;
                }

                GlobalContext.Properties["appname"] = AppDomain.CurrentDomain.FriendlyName;
            }
            catch (Exception e)
            {
                LogManager.GetLogger(GetType()).Error("An error occured attempting to set the application name for the logging context", e);
            }
        }

        private static ILog Logger(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = typeof(Log4NetLogService).Name;
            }

            return LogManager.GetLogger(name);
        }

        private static ILog Logger(Type source)
        {
            if (source == null)
            {
                source = typeof(Log4NetLogService);
            }

            return LogManager.GetLogger(source);
        }

        public void Debug(object source, object message)
        {
            Debug(source.GetType(), message);
        }

        public void Debug(Type source, object message)
        {
            Debug(source, message, null);
        }

        public void Debug(object source, object message, Exception exception)
        {
            Debug(source.GetType(), message, exception);
        }

        public void Debug(Type source, object message, Exception exception)
        {
            var log = Logger(source);

            if (log.IsDebugEnabled)
            {
                log.Debug(message, exception);
            }
        }

        public void Debug(string logName, object message, Exception exception = null)
        {
            var log = Logger(logName);

            if (log.IsDebugEnabled)
            {
                log.Debug(message, exception);
            }
        }

        public void Info(object source, object message)
        {
            Info(source.GetType(), message);
        }

        public void Info(Type source, object message)
        {
            Info(source, message, null);
        }

        public void Info(object source, object message, Exception exception)
        {
            Info(source.GetType(), message, exception);
        }

        public void Info(Type source, object message, Exception exception)
        {
            var log = Logger(source);

            if (log.IsInfoEnabled)
            {
                log.Info(message, exception);
            }
        }

        public void Info(string logName, object message, Exception exception = null)
        {
            var log = Logger(logName);

            if (log.IsInfoEnabled)
            {
                log.Info(message, exception);
            }
        }

        public void Warn(object source, object message)
        {
            Warn(source.GetType(), message);
        }

        public void Warn(Type source, object message)
        {
            Warn(source, message, null);
        }

        public void Warn(object source, object message, Exception exception)
        {
            Warn(source.GetType(), message, exception);
        }

        public void Warn(Type source, object message, Exception exception)
        {
            var log = Logger(source);

            if (log.IsWarnEnabled)
            {
                log.Warn(message, exception);
            }
        }

        public void Warn(string logName, object message, Exception exception = null)
        {
            var log = Logger(logName);

            if (log.IsWarnEnabled)
            {
                log.Warn(message, exception);
            }
        }

        public void Error(object source, object message)
        {
            Error(source.GetType(), message);
        }

        public void Error(Type source, object message)
        {
            Error(source, message, null);
        }

        public void Error(object source, object message, Exception exception)
        {
            Error(source.GetType(), message, exception);
        }

        public void Error(Type source, object message, Exception exception)
        {
            var log = Logger(source);

            if (log.IsErrorEnabled)
            {
                log.Error(message, exception);
            }
        }

        public void Error(string logName, object message, Exception exception = null)
        {
            var log = Logger(logName);

            if (log.IsErrorEnabled)
            {
                log.Error(message, exception);
            }
        }

        public void Fatal(object source, object message)
        {
            Fatal(source.GetType(), message);
        }

        public void Fatal(Type source, object message)
        {
            Fatal(source, message, null);
        }

        public void Fatal(object source, object message, Exception exception)
        {
            Fatal(source.GetType(), message, exception);
        }

        public void Fatal(Type source, object message, Exception exception)
        {
            var log = Logger(source);

            if (log.IsFatalEnabled)
            {
                log.Fatal(message, exception);
            }
        }

        public void Fatal(string logName, object message, Exception exception = null)
        {
            var log = Logger(logName);

            if (log.IsFatalEnabled)
            {
                log.Fatal(message, exception);
            }
        }

        private static long _isInitialized = 0;
        private static readonly object InitializationLock = new Object();
        private readonly object _lock = new object();
        private readonly Dictionary<string, ILog> _loggers = new Dictionary<string, ILog>();
    }
}
