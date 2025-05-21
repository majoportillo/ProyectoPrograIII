using System.Text;
using System.Text.Json;
using E_comerce.Services;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ECommerce.Api.Services
{
    public class OrderStatusConsumer : BackgroundService
    {
        private readonly E_comerce.Services.IConnection _connection;
        private readonly IModel _channel;
        private readonly EmailSender _emailSender;
        private readonly string _queueName;

        public OrderStatusConsumer(IConfiguration configuration, EmailSender emailSender)
        {
            _emailSender = emailSender;

            var factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQ:HostName"],
                UserName = configuration["RabbitMQ:UserName"],
                Password = configuration["RabbitMQ:Password"]
            };

            _queueName = configuration["RabbitMQ:QueueName"];

            _connection = factory.CreateConnectionAsync();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                // El mensaje es JSON con email y estado del pedido
                var data = JsonSerializer.Deserialize<OrderStatusMessage>(message);

                if (data != null)
                {
                    string subject = $"Estado de tu pedido: {data.Status}";
                    string bodyContent = $"Hola, tu pedido está ahora: <b>{data.Status}</b>.";

                    await _emailSender.SendEmailAsync(data.CustomerEmail, subject, bodyContent);
                }

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }

    public class OrderStatusMessage
    {
        public string CustomerEmail { get; set; }
        public string Status { get; set; }
    }
}
