using System;

namespace Contec.Framework.Logging
{
    public interface ILogService
    {
        void ApplicationName(string name);

        void Debug(object source, object message);
        void Debug(Type source, object message);
        void Debug(object source, object message, Exception exception);
        void Debug(Type source, object message, Exception exception);
        void Debug(string logName, object message, Exception exception = null);
        
        void Info(object source, object message);
        void Info(Type source, object message);
        void Info(object source, object message, Exception exception);
        void Info(Type source, object message, Exception exception);
        void Info(string logName, object message, Exception exception = null);
        
        void Warn(object source, object message);
        void Warn(Type source, object message);
        void Warn(object source, object message, Exception exception);
        void Warn(Type source, object message, Exception exception);
        void Warn(string logName, object message, Exception exception = null);
        
        void Error(object source, object message);
        void Error(Type source, object message);
        void Error(object source, object message, Exception exception);
        void Error(Type source, object message, Exception exception);
        void Error(string logName, object message, Exception exception = null);
        
        void Fatal(object source, object message);
        void Fatal(Type source, object message);
        void Fatal(object source, object message, Exception exception);
        void Fatal(Type source, object message, Exception exception);
        void Fatal(string logName, object message, Exception exception = null);
    }
}
