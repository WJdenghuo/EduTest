using Edu.Models.Models;
using Edu.Tools;
using EduTest.Controllers.API;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Receive
{
    class Program
    {
        public static async Task Main()
        {
            Console.WriteLine($"控制台程序已经启动:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\r\n"); 
            var hostBuilder = new HostBuilder()
           .ConfigureServices(serviceCollection => { //注册我们的服务接口
               serviceCollection.AddSingleton<IHostedService, MyService>();
           }); 
            await hostBuilder.RunConsoleAsync();
        }
    }

}
