using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Log.Extensions.Operation.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Log.Extensions.Operation.Middleware
{
    public class OperationMiddleware
    {
        private readonly RequestDelegate _next;
        public OperationMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context, ILogger<OperationMiddleware> logger)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            await _next(context);
            stopwatch.Stop();

            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            var controller = endpoint?.Metadata.OfType<ControllerDescAttribute>().FirstOrDefault();
            if (controller != null)
            {
                var action = endpoint.Metadata.OfType<ActionDescAttribute>().FirstOrDefault();
                var logMessage = new
                {
                    Ip = GetIp(context),
                    Name = endpoint.DisplayName,
                    Path = context.Request.Path.Value,
                    Type = "Operation",
                    ControllerName = controller.Name,
                    ActionName = action?.Name,
                    OperationTime = DateTime.Now,
                    Time = $"{stopwatch.ElapsedMilliseconds}ms",
                    User = context.User?.Identities
                };
                logger.LogInformation(JsonConvert.SerializeObject(logMessage));
            }
        }

        private string GetIp(HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
            return ip;
        }
    }
}
