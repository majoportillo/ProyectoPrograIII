using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using ECommerce.Api.Models;
using E_comerce.Models;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore.Metadata;

namespace E_comerce.Services
{
    public interface IQueueProducer
    {
        void Send(NotificationMessage message);
    }

    public class QueueProducerService : IQueueProducer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string QueueName = "order_notifications";

        public QueueProducerService()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: QueueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        public void Send(NotificationMessage message)
        {
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            // Fix: Ensure the correct RabbitMQ.Client.IModel is used
            _channel.BasicPublish(exchange: "",
                                  routingKey: QueueName,
                                  basicProperties: null,
                                  body: body);
        }
    }
}
