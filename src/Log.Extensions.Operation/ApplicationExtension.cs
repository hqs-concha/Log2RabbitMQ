using System;
using Log.Extensions.Operation.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Log.Extensions.Operation
{
    public static class ApplicationExtension
    {
        /// <summary>
        /// 记录访问日志 Controller -> Action；
        /// 该中间件请放置在UseRouting之后，UseEndpoints之前；
        /// 放置在UseAuthorization之后可获取到HttpContext中的用户信息（如果有授权）；
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseOperation(this IApplicationBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.UseMiddleware<OperationMiddleware>();
            return builder;
        }
    }
}
