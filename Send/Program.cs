using System;
using RabbitMQ.Client;
using System.Text;
using System.Threading;
using RabbitMQ.Client.Events;
using Edu.Tools;
using EduTest.Controllers.API;
using System.Collections.Concurrent;

namespace Send
{
    class Program
    {
        static void Main(string[] args)
        {
            var rpcClient = new RpcClient();

            DealCommand dealCommand = new DealCommand()
            {
                All = true
            };
            var body = JsonHelper.Serialize(dealCommand);
            Console.WriteLine($" [x] Requesting {body}");
            var response = rpcClient.Call(body);
            Console.WriteLine(" [.] Got '{0}'", response);

            rpcClient.Close();
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
        public class RpcClient
        {
            private readonly IConnection connection;
            private readonly IModel channel;
            private readonly string replyQueueName;
            private readonly EventingBasicConsumer consumer;
            private readonly BlockingCollection<string> respQueue = new BlockingCollection<string>();
            private readonly IBasicProperties props;

            public RpcClient()
            {
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
                replyQueueName = channel.QueueDeclare().QueueName;
                consumer = new EventingBasicConsumer(channel);

                props = channel.CreateBasicProperties();
                var correlationId = Guid.NewGuid().ToString();
                props.CorrelationId = correlationId;
                props.ReplyTo = replyQueueName;

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var response = Encoding.UTF8.GetString(body);
                    if (ea.BasicProperties.CorrelationId == correlationId)
                    {
                        respQueue.Add(response);
                    }
                };
            }

            public string Call(string message)
            {
                var messageBytes = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(
                    exchange: "",
                    routingKey: "rpc_queue",
                    basicProperties: props,
                    body: messageBytes);

                channel.BasicConsume(
                    consumer: consumer,
                    queue: replyQueueName,
                    autoAck: true);

                return respQueue.Take();
            }

            public void Close()
            {
                connection.Close();
            }
        }
    }
}
