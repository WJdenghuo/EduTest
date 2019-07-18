using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduTest.Infrastructure.Middlewares
{
    public class RequestUserMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public RequestUserMiddleware(RequestDelegate next,ILogger<RequestUserMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var test = context.Request.Query["test"];
            if (!String.IsNullOrWhiteSpace(test))
            {
                _logger.LogDebug($"中间件测试:{test}");
            }
            await _next(context);
        }
    }
}
