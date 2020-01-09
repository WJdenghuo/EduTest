using Edu.Models.Models;
using EduTest.Models.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Edu.Tools
{
    public class RpcServer
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string replyQueueName;
        private readonly EventingBasicConsumer consumer;
        private readonly IBasicProperties props;
        public RpcServer() 
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
            connection = factory.CreateConnection(new string[]
                {
                "49.233.130.117",
                "62.234.105.58"
                });
            channel = connection.CreateModel();
            channel.QueueDeclare(queue: "rpc_queue", durable: false,
                  exclusive: false, autoDelete: false, arguments: null);
            channel.BasicQos(0, 1, false);
            consumer = new EventingBasicConsumer(channel);
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
                    response = GetDealed(message);
                    //response = message;
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
        public void Close()
        {
            connection.Close();
        }
        static String GetDealed(String dec)
        {
            if (String.IsNullOrEmpty(dec))
            {
                return "";
            }
            else if (JsonHelper.DeserializeJsonToObject<DealCommand>(dec, out DealCommand dealCommand))
            {
                return Dealed(dealCommand);
            }
            return "";
        }
        static String Dealed(DealCommand dealCommand)
        {
            /*
             * 逻辑如下：
             * ①获取路径，拆分 roomID、userID、CreateTricks
             * ②根据dealCommand：a：all，该房间下所有用户；b：房间号+userIDs+tricks
             * ③返回处理后的路径
             */
            List<String> listPath = new List<string>();
            List<JanusRecordMedia> janusRecordMedias = new List<JanusRecordMedia>();
            List<String> listDealedPath = new List<string>();
            Directory.GetFiles("/test")
                .Where(x => Path.GetExtension(x) == ".mjr").ToList()?
                .ForEach(x =>
                {
                    var media = x.Split('-');
                    janusRecordMedias.Add(new JanusRecordMedia()
                    {
                        FileName = x,
                        RoomID = media[1],
                        UserID = media[3],
                        CreateTricks = Int64.TryParse(media[5][0..^3], out long result) ? result : DateTime.UtcNow.Ticks
                    });
                });
            if (dealCommand.All)
            {
                if (String.IsNullOrEmpty(dealCommand.RoomID))
                {
                    janusRecordMedias?
                    .ForEach(x =>
                    {
                        if (x.FileName.Contains("video.mjr"))
                        {
                            Deal("janus-pp-rec", $"{x.FileName} {x.FileName[0..^3]}webm");
                        }
                        else if (x.FileName.Contains("audio.mjr"))
                        {
                            Deal("janus-pp-rec", $"{x.FileName} {x.FileName[0..^3]}opus");
                        }
                        listPath.Add(x.FileName);
                    });
                }
                else
                {
                    janusRecordMedias?
                    .Where(x => x.RoomID == dealCommand.RoomID)
                    .ToList()
                    .ForEach(x =>
                    {
                        if (x.FileName.Contains("video.mjr"))
                        {
                            Deal("janus-pp-rec", $"{x.FileName} {x.FileName[0..^3]}webm");
                        }
                        else if (x.FileName.Contains("audio.mjr"))
                        {
                            Deal("janus-pp-rec", $"{x.FileName} {x.FileName[0..^3]}opus");
                        }
                        listPath.Add(x.FileName);
                    });
                }

            }
            else
            {
                janusRecordMedias?
                    .Where(x => x.CreateTricks >= DateTime.Today.Ticks &&
                    DateTime.Today.AddDays(1).Ticks > x.CreateTricks &&
                    dealCommand.UserIDs.Contains(x.UserID) &&
                    x.RoomID == dealCommand.RoomID)
                    .ToList()
                    .ForEach(x =>
                    {
                        if (x.FileName.Contains("video.mjr"))
                        {
                            Deal("janus-pp-rec", $"{x.FileName} {x.FileName[0..^3]}webm");
                        }
                        else if (x.FileName.Contains("audio.mjr"))
                        {
                            Deal("janus-pp-rec", $"{x.FileName} {x.FileName[0..^3]}opus");
                        }
                        listPath.Add(x.FileName);
                    });
            }
            listPath.Where(x => x.Contains("video.mjr")).ToList().ForEach(x =>
            {
                Deal("ffmpeg", $"-i {x[0..^9]}audio.opus -i {x[0..^3]}webm  -c:v copy -c:a opus -y -strict experimental {x[0..^4]}-hasDeal.webm");
                listDealedPath.Add(x[0..^4] + "-hasDeal.webm");
            });
            return Edu.Tools.JsonHelper.Serialize(listDealedPath);
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
