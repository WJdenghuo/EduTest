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

namespace Receive
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                //HostName= "106.13.116.83",
                UserName = "admin",
                Password = "admin",
                AutomaticRecoveryEnabled = true,
                TopologyRecoveryEnabled = true
            };
            using var connection = factory.CreateConnection(new string[]
            {
                "106.13.116.83",
                "62.234.105.58"
            });
            using var channel = connection.CreateModel();

            //#region 简单应用
            //channel.QueueDeclare(queue: "hello1",
            //                    durable: false,
            //                    exclusive: false,
            //                    autoDelete: false,
            //                    arguments: null);
            //var consumer = new EventingBasicConsumer(channel);
            //consumer.Received += (model, ea) =>
            //{
            //    var body = ea.Body;
            //    var message = Encoding.UTF8.GetString(body);
            //    Console.WriteLine(" [x] Received {0}", message);
            //};
            //channel.BasicConsume(queue: "hello1",
            //                     autoAck: true,
            //                     consumer: consumer);
            //#endregion


            #region RPC
            //申明队列接收远程调用请求
            channel.QueueDeclare(queue: "rpc_queue", durable: false,
                exclusive: false, autoDelete: false, arguments: null);
            var consumer = new EventingBasicConsumer(channel);
            Console.WriteLine("[*] Waiting for message.");
            //请求处理逻辑
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body);

                DealCommand dealCommand = new DealCommand();
                dealCommand = JsonHelper.Deserialize<DealCommand>(message);
                if (dealCommand.All)
                {
                    Deal();
                }
                var test = Directory.GetFiles("/test").Where(x => x.Contains("hasDeal.webm")).ToList();
                List<JanusRecordMedia> janusRecordMedias = new List<JanusRecordMedia>();
                test.ForEach(x =>
                {
                    janusRecordMedias.Add(new JanusRecordMedia() { FileName = x });
                });
                //Thread.Sleep(12000);//模拟耗时
                //从请求的参数中获取请求的唯一标识，在消息回传时同样绑定
                var properties = ea.BasicProperties;
                var replyProerties = channel.CreateBasicProperties();
                replyProerties.CorrelationId = properties.CorrelationId;
                //将远程调用结果发送到客户端监听的队列上
                channel.BasicPublish(exchange: "", routingKey: properties.ReplyTo,
                basicProperties: replyProerties, body: Encoding.UTF8.GetBytes(JsonHelper.Serialize(test)));
                //手动发回消息确认
                channel.BasicAck(ea.DeliveryTag, false);
                //Console.WriteLine($"Return result: Fib({n})= {result}");
            };
            channel.BasicConsume(queue: "rpc_queue", autoAck: false, consumer: consumer);
            #endregion
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        static void Deal()
        {
            var test = Directory.GetFiles("/test").Where(x => Path.GetExtension(x) == ".mjr").ToList();
            if (test.Count > 0)
            {
                test.ForEach(x =>
                {
                    Console.WriteLine($"{x}");
                    if (x.Contains("video.mjr"))
                    {
                        Deal("janus-pp-rec", $"{x} {x.Substring(0, x.Length - 3)}webm");
                    }
                    else if (x.Contains("audio.mjr"))
                    {
                        Deal("janus-pp-rec", $"{x} {x.Substring(0, x.Length - 3)}opus");
                    }
                });
            }
            test.Where(x => x.Contains("video.mjr")).ToList().ForEach(x =>
            {
                Deal("ffmpeg", $"-i {x.Substring(0, x.Length - 9)}audio.opus -i {x.Substring(0, x.Length - 3)}webm  -c:v copy -c:a opus -strict experimental {x.Substring(0, x.Length - 4)}-hasDeal.webm");
            });
            Console.WriteLine("Hello World!");


        }
        static void Deal(String fileName, string args)
        {
            #region process 测试
            var psi = new ProcessStartInfo(fileName, args) { RedirectStandardOutput = true };
            Console.WriteLine($"{fileName} {args}");
            //启动
            var proc = Process.Start(psi);
            if (proc == null)
            {
                Console.WriteLine("Can not exec.");
            }
            else
            {
                Console.WriteLine("-------------Start read standard output--------------");
                //开始读取
                using (var sr = proc.StandardOutput)
                {
                    while (!sr.EndOfStream)
                    {
                        Console.WriteLine(sr.ReadLine());
                    }

                    if (!proc.HasExited)
                    {
                        proc.Kill();
                    }
                }
                Console.WriteLine("---------------Read end------------------");
                //Console.WriteLine($"Total execute time :{(proc.ExitTime - proc.StartTime).TotalMilliseconds} ms");
                //Console.WriteLine($"Exited Code ： {proc.ExitCode}");
            }
            #endregion
        }
    }
}
