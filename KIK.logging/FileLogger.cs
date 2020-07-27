using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace KIK.logging
{
    public sealed class FileLogger : ILogger
    {
        private string _categoryName;
        private Func<string, LogLevel, bool> _filter;
        private string _fileName;
        private FileLoggerHelper _helper;

        public FileLogger(string catName, Func<string,LogLevel,bool> filter, string filename)
        {
            _fileName = filename;
            _helper = new FileLoggerHelper(filename);
        }



        IDisposable ILogger.BeginScope<TState>(TState state)
        {
            return null;
        }

        bool ILogger.IsEnabled(LogLevel logLevel)
        {
            return (_filter == null || _filter(_categoryName, logLevel));
        }

        void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception);

            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            if(exception!=null)
            {
                message += "\n" + exception.ToString();
            }

            var logEntry = new LogEntry
            {
                Message = message,
                EventId = eventId.Id,
                LogLevel = logLevel.ToString(),
                CreatedTime = DateTime.UtcNow
            };

            _helper.InsertLog(logEntry);
        }

        private bool IsEnabled(LogLevel logLevel)
        {
            return (_filter == null || _filter(_categoryName, logLevel));
        }
    }
}
