using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ECommerce.Api.Models;
using System.Net;
using System.Net.Mail;
using E_comerce.Services;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ECommerce.Api.Services
{
    public class QueueConsumerService
    {
        private readonly IModel _channel;
        private readonly IConnection _connection;
        private const string QueueName = "order_notifications";

        public QueueConsumerService()
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

        public void Start()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);
                var message = JsonSerializer.Deserialize<NotificationMessage>(messageJson);

                if (message != null)
                {
                    await SendEmailAsync(message.Email, message.Subject, message.Body);
                }

                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(queue: QueueName,
                                 autoAck: false,
                                 consumer: consumer);
        }

        private async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("tuemail@gmail.com", "tuContraseña"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage("tuemail@gmail.com", to, subject, body);
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
