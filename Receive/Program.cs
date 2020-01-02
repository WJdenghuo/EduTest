using Edu.Models.Models;
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
        public static void Main()
        {
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
                "106.13.116.83",
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
                        //response = GetDealed();
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

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        /// 

        /// Assumes only valid positive integer input.
        /// Don't expect this one to work for big numbers, and it's
        /// probably the slowest recursive implementation possible.
        /// 
        private static int Fib(int n)
        {
            if (n == 0 || n == 1)
            {
                return n;
            }

            return Fib(n - 1) + Fib(n - 2);
        }

        static String GetDealed()
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
                Deal("ffmpeg", $"-i {x[0..^9]}audio.opus -i {x[0..^3]}webm  -c:v copy -c:a opus -y -strict experimental {x[0..^4]}-hasDeal.webm");
            });
            return Edu.Tools.JsonHelper.Serialize(test);
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
