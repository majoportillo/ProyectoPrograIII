using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MimeKit;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SuperBodega.EmailWorker.Dtos;
using System.Text;
using System.Text.Json;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private IConnection _connection;
    private IModel _channel;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;

        var factory = new ConnectionFactory() { HostName = "localhost" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: "notificaciones",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var mensaje = Encoding.UTF8.GetString(body);

            _logger.LogInformation("📩 Mensaje recibido: {Mensaje}", mensaje);

            var notificacion = JsonSerializer.Deserialize<NotificacionDto>(mensaje);
            if (notificacion is not null)
            {
                await EnviarCorreoAsync(notificacion);
            }
        };

        _channel.BasicConsume(queue: "notificaciones", autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }

    private async Task EnviarCorreoAsync(NotificacionDto dto)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("tucorreo@gmail.com"));
        email.To.Add(MailboxAddress.Parse(dto.Email));
        email.Subject = $"🛒 Estado de tu pedido: {dto.Estado}";

        string mensaje = dto.Estado switch
        {
            "Recibido" => $"Hola,\n\nHemos recibido tu pedido y pronto será procesado. 🎉\n\nTotal: Q{dto.Total}\n\nGracias por tu compra.\nSuperBodega",
            "Despachado" => $"Hola,\n\nTu pedido ha sido despachado y está en camino. 🚚\n\nGracias por tu preferencia.\nSuperBodega",
            "Entregado" => $"Hola,\n\nTu pedido ha sido entregado con éxito. 📬\n\nEsperamos que lo disfrutes.\nSuperBodega",
            _ => $"Hola,\n\nEl estado de tu pedido ha cambiado: {dto.Estado}\n\nGracias por tu compra.\nSuperBodega"
        };

        email.Body = new TextPart("plain") { Text = mensaje };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync("tucorreo@gmail.com", "TU_APP_PASSWORD");
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }

}


