using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Entities.Concrete;

namespace Business.RabbitMQ.Consumer
{
    public class RabbitMQConsumer
    {
        public static async Task<List<Order>> ReadMessages()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var orders = new List<Order>();

            try
            {
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "auditlog",
                                          durable: false,
                                          exclusive: false,
                                          autoDelete: false,
                                          arguments: null);

                    while (true)
                    {
                        BasicGetResult result = await Task.Run(() => channel.BasicGet(queue: "auditlog", autoAck: true));
                        Console.WriteLine($"Result: {result}");

                        if (result == null)
                        {
                            Console.WriteLine("No more messages in queue.");
                            break;
                        }

                        var body = result.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine($" [x] Received: {message}");

                        try
                        {
                            var order = JsonSerializer.Deserialize<Order>(message);
                            if (order != null)
                            {
                                orders.Add(order);
                                Console.WriteLine($" [x] Deserialized Order: {order}");
                            }
                        }
                        catch (JsonException jsonEx)
                        {
                            Console.WriteLine($" [!] JSON Deserialization Error: {jsonEx.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" [!] Exception: {ex.Message}");
            }

            return orders;
        }
    }
}
