using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Log2RabbitMq
{
    public class RabbitMqLoggerProvider : ILoggerProvider, IDisposable
    {
        RabbitMqLoggerSettings _configuration;
        readonly ConcurrentDictionary<string, RabbitMqLogger> _loggers = new ConcurrentDictionary<string, RabbitMqLogger>();

        public RabbitMqLoggerProvider(RabbitMqLoggerSettings configuration)
        {
            _configuration = configuration;
            _configuration.ChangeToken.RegisterChangeCallback(p =>
            {
                _configuration.Reload();
            }, null);
        }

        public void Dispose()
        {
            
        }

        public ILogger CreateLogger(string categoryName)
        {
            return this._loggers.GetOrAdd(categoryName, p =>
                {
                    var logger = new RabbitMqLogger(categoryName, _configuration);
                    return logger;
                });
        }
    }
}
