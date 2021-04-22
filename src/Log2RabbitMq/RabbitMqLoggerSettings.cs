using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Log2RabbitMq
{
    public class RabbitMqLoggerSettings
    {
        IConfiguration _configuration;
        public IChangeToken ChangeToken { get; private set; }
        public RabbitMqLoggerSettings(IConfiguration configuration)
        {
            _configuration = configuration;
            this.ChangeToken = _configuration.GetReloadToken();
        }

        public string HostName => _configuration["Log2RabbitMq:HostName"];

        public int Port
        {
            get
            {
                if (int.TryParse(_configuration["Log2RabbitMq:Port"], out int port))
                {
                    return port;
                }

                return 5672;
            }
        }

        public string UserName => _configuration["Log2RabbitMq:UserName"];
        public string Password => _configuration["Log2RabbitMq:Password"];
        public string AppName => _configuration["Log2RabbitMq:AppName"];

        public Tuple<bool, LogLevel> GetSwitch(string name)
        {
            var section = this._configuration.GetSection("Log2RabbitMq").GetSection("LogLevel");
            if (section != null)
            {
                if (Enum.TryParse(section[name], true, out LogLevel level))
                    return new Tuple<bool, LogLevel>(true, level);
            }
            return new Tuple<bool, LogLevel>(false, LogLevel.None);
        }
        public void Reload()
        {
            //update cache settings
        }
    }
}
