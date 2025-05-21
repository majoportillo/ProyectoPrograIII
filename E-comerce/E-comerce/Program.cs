using ECommerce.Api.Data;
using Microsoft.EntityFrameworkCore;
using ECommerce.Api.Services;
using E_comerce.Services;

var builder = WebApplication.CreateBuilder(args);


// Add services
builder.Services.AddDbContext<ECommerceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddSingleton<IQueueProducer, QueueProducerService>();
builder.Services.AddSingleton<QueueConsumerService>();
builder.Services.AddHttpClient<EmailSender>();
builder.Services.AddSingleton<OrderStatusPublisher>();
builder.Services.AddHostedService<OrderStatusConsumer>();


var app = builder.Build();

// Use middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Iniciar consumidor de colas en segundo plano
var queueConsumer = app.Services.GetRequiredService<QueueConsumerService>();
queueConsumer.Start();

// Middleware, endpoints, etc.
app.UseAuthorization();
app.MapControllers();

app.Run();
app.UseAuthorization();
app.MapControllers();
app.Run();
