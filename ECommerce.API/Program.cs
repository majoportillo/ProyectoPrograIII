using ECommerce.API.Data;
using ECommerce.API.Services;
using ECommerce.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using SuperBodega.Models.Dtos;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<ECommerceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Servicios
builder.Services.AddScoped<IProductoCatalogoService, ProductoCatalogoService>();
builder.Services.AddScoped<ICarritoService, CarritoService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddAuthorization(); 
builder.Services.AddControllers(); 


var app = builder.Build();

// Swagger middleware
app.UseSwagger();
app.UseSwaggerUI();


// CORS y demás
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();


