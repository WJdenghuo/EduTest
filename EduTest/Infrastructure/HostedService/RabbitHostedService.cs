using Edu.Tools;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EduTest.Infrastructure.HostedService
{
    internal class RabbitHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private RpcServer _rpcServer;
        public RabbitHostedService(ILogger<RabbitHostedService> logger) 
        {
            _logger = logger;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _rpcServer = new RpcServer();
            _logger.LogInformation("RpcServer-Rabbitmq 启动");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _rpcServer.Close();
            _logger.LogInformation("RpcServer-Rabbitmq 停止");
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _rpcServer.Close();
        }
    }
}
