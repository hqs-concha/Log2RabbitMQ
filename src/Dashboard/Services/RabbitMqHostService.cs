using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dashboard.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Dashboard.Services
{
    public class RabbitMqHostService : BackgroundService
    {
        private readonly RabbitMqSettings _mqSettings;
        private readonly LogService _logService;
        private readonly ILogger<RabbitMqHostService> _logger;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMqHostService(LogService logService, IOptions<RabbitMqSettings> options,
            ILogger<RabbitMqHostService> logger)
        {
            _logService = logService;
            _logger = logger;
            _mqSettings = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            InitRabbitMq();
            while (!stoppingToken.IsCancellationRequested)
            {
                _channel.QueueDeclare(queue: _mqSettings.Queue,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                var consumer = new EventingBasicConsumer(_channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    AddLog2Mongo(message);
                    _logger.LogInformation($"接受到消息【{message}】");
                };

                _channel.BasicConsume(queue: _mqSettings.Queue,
                    autoAck: true,
                    consumer: consumer);

                await Task.Delay(TimeSpan.FromMilliseconds(1), stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken stoppingToken)
        {
            _channel.Close();
            _connection.Close();
            _logger.LogInformation($"++++++++++已关闭++++++++++");
            return Task.CompletedTask;
        }

        private void InitRabbitMq()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _mqSettings.HostName,
                UserName = _mqSettings.UserName,
                Password = _mqSettings.Password,
                Port = _mqSettings.Port
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _logger.LogInformation($"++++++++++已就绪++++++++++");
        }

        private void AddLog2Mongo(string message)
        {
            var logModel = JsonConvert.DeserializeObject<LogModel>(message);
            _logService.Create(logModel);
        }
    }
}
