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
                .ToListAsync();

            var result = new List<VentaDto>();
            foreach (var venta in ventas)
            {
                var dto = _mapper.Map<VentaDto>(venta);
                dto.Detalle = _mapper.Map<List<DetalleVentaDto>>(venta.DetalleVenta);
                result.Add(dto);
            }

            return result;
        }

        public async Task<VentaDto?> GetByIdAsync(int id)
        {
            var venta = await _context.Ventas
                .Include(v => v.DetalleVenta)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venta == null) return null;

            var dto = _mapper.Map<VentaDto>(venta);
            dto.Detalle = _mapper.Map<List<DetalleVentaDto>>(venta.DetalleVenta);
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

                // ✅ Restar stock
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

            venta.Estado = nuevoEstado;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

