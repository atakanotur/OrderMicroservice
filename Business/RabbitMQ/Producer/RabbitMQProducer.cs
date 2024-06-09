using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.RabbitMQ.Producer
{
    public class RabbitMQProducer
    {
        public static void SendMessage<T>(T message)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
            };
            var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "auditlog", durable: false,exclusive: false, autoDelete: false, arguments: null);
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(exchange: "", routingKey: "auditlog", basicProperties: null, body: body);
        }
    }
}
