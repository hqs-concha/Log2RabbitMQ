using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Log2RabbitMq
{
    public static class RabbitMqLoggerExtensions
    {
        public static ILoggingBuilder AddLog2RabbitMq(this ILoggingBuilder build, IConfiguration configuration)
        {
            return AddLog2RabbitMq(build, new RabbitMqLoggerSettings(configuration));
        }

        private static ILoggingBuilder AddLog2RabbitMq(this ILoggingBuilder builder, RabbitMqLoggerSettings kafkaLoggerSettings)
        {
            builder.AddProvider(new RabbitMqLoggerProvider(kafkaLoggerSettings));
            return builder;
        }
    }
}
