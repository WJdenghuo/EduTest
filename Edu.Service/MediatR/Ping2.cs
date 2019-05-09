using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Edu.Service.MediatR
{
    //构建 通知消息
    public class Ping2 : INotification { }
    //构建 消息处理器1
    public class Pong1 : INotificationHandler<Ping2>
    {
        public Task Handle(Ping2 notification, CancellationToken cancellationToken)
        {
            Debug.WriteLine("Pong 1");
            return Task.CompletedTask;
        }
    }
    //构建 消息处理器2
    public class Pong2 : INotificationHandler<Ping2>
    {
        public Task Handle(Ping2 notification, CancellationToken cancellationToken)
        {
            Debug.WriteLine("Pong 2");
            return Task.CompletedTask;
        }
    }
}
