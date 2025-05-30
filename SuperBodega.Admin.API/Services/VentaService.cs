using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SuperBodega.Admin.API.Data;
using SuperBodega.Admin.API.Dtos;
using SuperBodega.Admin.API.Models;
using SuperBodega.Admin.API.Services.Interfaces;
using SuperBodega.Models;

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
            var ventas = await _context.Ventas
                .Include(v => v.DetalleVenta)
                    .ThenInclude(dv => dv.Producto)
                .ToListAsync();

            var result = new List<VentaDto>();
            foreach (var venta in ventas)
            {
                var dto = _mapper.Map<VentaDto>(venta);
                dto.Detalle = venta.DetalleVenta.Select(d => new DetalleVentaDto
                {
                    ProductoId = d.ProductoId,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    NombreProducto = d.Producto?.Nombre
                }).ToList();
                result.Add(dto);
            }

            return result;
        }


            public async Task<VentaDto?> GetByIdAsync(int id)
            {
                var venta = await _context.Ventas
                    .Include(v => v.DetalleVenta)
                        .ThenInclude(dv => dv.Producto)
                    .FirstOrDefaultAsync(v => v.Id == id);

                if (venta == null) return null;

                var dto = _mapper.Map<VentaDto>(venta);
                dto.Detalle = venta.DetalleVenta.Select(d => new DetalleVentaDto
                {
                    ProductoId = d.ProductoId,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    NombreProducto = d.Producto?.Nombre
                }).ToList();

                return dto;
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
            await _context.SaveChangesAsync(); // Necesario para obtener el ID

            var detallesAgrupados = dto.Detalle
    .GroupBy(d => d.ProductoId)
    .Select(g => new DetalleVentaDto
    {
        ProductoId = g.Key,
        Cantidad = g.Sum(x => x.Cantidad),
        PrecioUnitario = g.First().PrecioUnitario
    });

            foreach (var detalle in detallesAgrupados)
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

            var result = _mapper.Map<VentaDto>(venta);
            result.Detalle = dto.Detalle;
            return result;
        }

        public async Task<bool> CambiarEstadoAsync(int id, string nuevoEstado)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null) return false;

            var estadosValidos = new[] { "Recibido", "Despachado", "Entregado" };

            if (!estadosValidos.Contains(nuevoEstado))
                return false;

            venta.Estado = nuevoEstado;
            await _context.SaveChangesAsync();
            return true;
        }

    }
}

