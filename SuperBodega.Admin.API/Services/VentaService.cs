// Archivo: SuperBodega.Admin.API/Services/VentaService.cs
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SuperBodega.Admin.API.Data;
using SuperBodega.Admin.API.Dtos;
using SuperBodega.Admin.API.Models;
using SuperBodega.Admin.API.Services.Interfaces;
using SuperBodega.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using SuperBodega.EmailWorker.Dtos;


namespace SuperBodega.Admin.API.Services
{
    public class VentaService : IVentaService
    {
        private readonly SuperBodegaDbContext _context;
        private readonly IMapper _mapper;

        public VentaService(SuperBodegaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VentaDto>> GetAllAsync()
        {
            var ventas = await _context.Ventas.Include(v => v.DetalleVenta).ThenInclude(dv => dv.Producto).ToListAsync();
            return ventas.Select(v => new VentaDto
            {
                Id = v.Id,
                Fecha = v.Fecha,
                Estado = v.Estado,
                ClienteId = v.ClienteId,
                Total = v.Total,
                Detalle = v.DetalleVenta.Select(d => new DetalleVentaDto
                {
                    ProductoId = d.ProductoId,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    NombreProducto = d.Producto?.Nombre
                }).ToList()
            });
        }

        public async Task<VentaDto?> GetByIdAsync(int id)
        {
            var venta = await _context.Ventas.Include(v => v.DetalleVenta).ThenInclude(dv => dv.Producto).FirstOrDefaultAsync(v => v.Id == id);
            if (venta == null) return null;
            return new VentaDto
            {
                Id = venta.Id,
                Fecha = venta.Fecha,
                Estado = venta.Estado,
                ClienteId = venta.ClienteId,
                Total = venta.Total,
                Detalle = venta.DetalleVenta.Select(d => new DetalleVentaDto
                {
                    ProductoId = d.ProductoId,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    NombreProducto = d.Producto?.Nombre
                }).ToList()
            };
        }

        public async Task<VentaDto> CreateAsync(VentaCreateDto dto)
        {
            var venta = new Venta
            {
                Fecha = DateTime.Now,
                Estado = "Recibido",
                ClienteId = dto.ClienteId,
                Total = dto.Detalle.Sum(d => d.PrecioUnitario * d.Cantidad)
            };

            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            foreach (var detalle in dto.Detalle)
            {
                var detalleVenta = new DetalleVenta
                {
                    VentaId = venta.Id,
                    ProductoId = detalle.ProductoId,
                    Cantidad = detalle.Cantidad,
                    PrecioUnitario = detalle.PrecioUnitario
                };

                _context.DetalleVenta.Add(detalleVenta);

                var producto = await _context.Productos.FindAsync(detalle.ProductoId);
                if (producto != null)
                {
                    producto.Stock = Math.Max(0, (producto.Stock ?? 0) - detalle.Cantidad);
                }
            }

            await _context.SaveChangesAsync();

            return _mapper.Map<VentaDto>(venta);
        }

        public async Task<bool> CambiarEstadoAsync(int id, string nuevoEstado)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null) return false;

            venta.Estado = nuevoEstado;
            await _context.SaveChangesAsync();

            var cliente = await _context.Clientes.FindAsync(venta.ClienteId);
            if (cliente != null)
            {
                var emailDto = new EmailVentaDto
                {
                    CorreoCliente = cliente.Email,
                    Total = venta.Total,
                    Estado = nuevoEstado
                };

                var factory = new ConnectionFactory() { HostName = "localhost" };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(queue: "notificaciones_ventas", durable: false, exclusive: false, autoDelete: false);
                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(emailDto));
                channel.BasicPublish(exchange: "", routingKey: "notificaciones_ventas", body: body);
            }

            return true;
        }
    }
}


