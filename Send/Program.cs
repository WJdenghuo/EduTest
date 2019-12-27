using System;
using RabbitMQ.Client;
using System.Text;
using System.Threading;
using RabbitMQ.Client.Events;
using Edu.Tools;
using EduTest.Controllers.API;

namespace Send
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                //HostName = "106.13.116.83",//62.234.105.58,106.13.116.83
                UserName = "admin",
                Password = "admin",
                AutomaticRecoveryEnabled = true,
                TopologyRecoveryEnabled = true
            };
            //集群在connection中添加地址
            using var connection = factory.CreateConnection(new string[]
            {
                "106.13.116.83",
                "62.234.105.58"
            });
            using (var channel = connection.CreateModel())
            {
                //#region 简单逻辑
                ////申明队列(指定durable: true, 告知rabbitmq对消息进行持久化)
                //channel.QueueDeclare(queue: "hello1",
                //                     durable: false,
                //                     exclusive: false,
                //                     autoDelete: false,
                //                     arguments: null);
                ////将消息标记为持久性 - 将IBasicProperties.SetPersistent设置为true
                //var properties = channel.CreateBasicProperties();
                //properties.Persistent = true;

                ////设置prefetchCount : 1来告知RabbitMQ，在未收到消费端的消息确认时，不再分发消息，也就确保了当消费端处于忙碌状态时
                //channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                //var testdate = DateTime.Now.AddMinutes(1);
                //for (var i = 1; i < 100000; i++)
                //{
                //    string message = $"{i}Hello World!";
                //    var body = Encoding.UTF8.GetBytes(message);

                //    channel.BasicPublish(exchange: "",
                //                         routingKey: "hello1",
                //                         basicProperties: null,
                //                         body: body);

                //    Console.WriteLine(" [x] Sent {0}", message);
                //}
                //#endregion



                #region RPC
                // 申明唯一guid用来标识此次发送的远程调用请求
                var correlationId = Guid.NewGuid().ToString();
                //申明需要监听的回调队列
                var replyQueue = channel.QueueDeclare().QueueName;
                var properties = channel.CreateBasicProperties();
                properties.ReplyTo = replyQueue;//指定回调队列
                properties.CorrelationId = correlationId;//指定消息唯一标识
                //string number = args.Length > 0 ? args[0] : "30";
                //处理所有文件
                DealCommand dealCommand = new DealCommand()
                {
                    All = true
                };
                var body = Encoding.UTF8.GetBytes(JsonHelper.Serialize(dealCommand));

                // //创建消费者用于处理消息回调（远程调用返回结果）
                var callbackConsumer = new EventingBasicConsumer(channel);
                channel.BasicConsume(queue: replyQueue, autoAck: true, consumer: callbackConsumer);
                callbackConsumer.Received += (model, ea) =>
                {
                    //仅当消息回调的ID与发送的ID一致时，说明远程调用结果正确返回。
                    if (ea.BasicProperties.CorrelationId == correlationId)
                    {
                        var responseMsg = $"Get Response: {Encoding.UTF8.GetString(ea.Body)}";
                        Console.WriteLine($"[x]: {responseMsg}");
                    }
                };

                /*
                 * ----------------------------------------
                 * 
                 * 发现一个bug 客户端发布消息位于用于回调的消费者之前会出现接收不到
                 * 远端消费的回调，所以消费要限于生产
                 * 
                 * ----------------------------------------
                 */
                //发布消息
                channel.BasicPublish(exchange: "", routingKey: "rpc_queue", basicProperties: properties, body: body);
                //Console.WriteLine($"[*] Request fib({number})");
                #endregion
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
