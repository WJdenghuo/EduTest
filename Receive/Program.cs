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
        public static async Task Main(string[] args)
        {
            Console.WriteLine($"控制台程序已经启动:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\r\n");
            var hostBuilder = new HostBuilder()
           .ConfigureServices(serviceCollection =>
           { //注册我们的服务接口
                serviceCollection.AddSingleton<IHostedService, MyService>();
           });
            await hostBuilder.RunConsoleAsync();
            //Start();
            //Console.WriteLine(" Press [enter] to exit.");
            //Console.ReadLine();
        }
        public static void Start()
        {
            //生产项目的话，把这块逻辑抽出来，增加重试策略（policy）
            var factory = new ConnectionFactory()
            {
                //HostName= "106.13.116.83",
                UserName = "admin",
                Password = "admin",
                AutomaticRecoveryEnabled = true,
                TopologyRecoveryEnabled = true
            };
            using (var connection = factory.CreateConnection(new string[]
                {
                "49.233.130.117",
                "62.234.105.58"
                }))
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "rpc_queue", durable: false,
                  exclusive: false, autoDelete: false, arguments: null);
                channel.BasicQos(0, 1, false);
                var consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume(queue: "rpc_queue",
                  autoAck: false, consumer: consumer);
                Console.WriteLine(" [x] Awaiting RPC requests");

                consumer.Received += (model, ea) =>
                {
                    string response = null;

                    var body = ea.Body;
                    var props = ea.BasicProperties;
                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    try
                    {
                        var message = Encoding.UTF8.GetString(body);

                        Console.WriteLine($"receive message{message}");
                        //response = GetDealed(message);
                        response = message;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(" [.] " + e.Message);
                        response = "";
                    }
                    finally
                    {
                        var responseBytes = Encoding.UTF8.GetBytes(response);
                        channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                          basicProperties: replyProps, body: responseBytes);
                        channel.BasicAck(deliveryTag: ea.DeliveryTag,
                          multiple: false);
                    }
                };
            }

        }
    }

}
