using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduTest.Infrastructure.Middlewares
{
    public static class RequestUserMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestUser(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestUserMiddleware>();
        }
    }
}
