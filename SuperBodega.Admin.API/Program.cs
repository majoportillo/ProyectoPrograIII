using Microsoft.EntityFrameworkCore;
using SuperBodega.Admin.API.Data;
using SuperBodega.Admin.API.Mappings;
using SuperBodega.Admin.API.Services;
using SuperBodega.Admin.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 🔌 Conexion a SQL Server (ajusta si cambias el puerto o usuario de Docker)
builder.Services.AddDbContext<SuperBodegaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Registro de servicios (inyeccion de dependencias)
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IProveedorService, ProveedorService>();
builder.Services.AddScoped<ICompraService, CompraService>();
builder.Services.AddScoped<IVentaService, VentaService>();
builder.Services.AddScoped<ICarritoService, CarritoService>();
builder.Services.AddScoped<IReporteService, ReporteService>();


// 🧠 AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// 🌐 Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔧 Controladores
builder.Services.AddControllers();

var app = builder.Build();

// 🧪 Swagger solo en desarrollo

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();


