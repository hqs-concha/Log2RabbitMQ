using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Log2RabbitMq
{
    public class RabbitMqLogger : ILogger
    {
        private readonly RabbitMqLoggerSettings _configuration;
        public RabbitMqLogger(string categoryName, RabbitMqLoggerSettings configuration)
        {
            this.Name = categoryName;
            _configuration = configuration;
        }
        public string Name { get; private set; }
        

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!this.IsEnabled(logLevel))
                return;
            var msg = formatter(state, exception);
            this.Write2RabbitMq(logLevel, eventId, msg, exception);
        }


        private void Write2RabbitMq(LogLevel logLevel, EventId eventId, string msg, Exception exception)
        {
            var data = new
            {
                MessageId = Guid.NewGuid(),
                LogTime = DateTime.Now,
                AppName = _configuration.AppName,
                CategoryName = Name,
                EventId = JsonConvert.SerializeObject(eventId),
                LogLevel = logLevel.ToString(),
                Message = msg,
                Exception = exception != null ? JsonConvert.SerializeObject(exception) : string.Empty,
            };
            SendRabbitMq(data);
        }

        private void SendRabbitMq(object data)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration.HostName,
                Port = _configuration.Port,
                UserName = _configuration.UserName,
                Password = _configuration.Password
            };

            var rabbitMqKey = "service.rabbit.queue";
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: rabbitMqKey,
                        type: ExchangeType.Direct,
                        durable: true,
                        autoDelete: false,
                        arguments: null);

                    channel.QueueDeclare(queue: rabbitMqKey,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    channel.QueueBind(queue: rabbitMqKey, exchange: rabbitMqKey, routingKey: rabbitMqKey);

                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

                    channel.BasicPublish(exchange: rabbitMqKey,
                        routingKey: rabbitMqKey,
                        basicProperties: null,
                        body: body);
                }
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            var keys = Name.Split('.');
            foreach (var item in keys)
            {
                var switchV = _configuration.GetSwitch(item);
                if (switchV.Item1)
                {
                    return logLevel >= switchV.Item2;
                }
            }
            var defaultSwitch = _configuration.GetSwitch("Default");
            return logLevel >= defaultSwitch.Item2;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}
