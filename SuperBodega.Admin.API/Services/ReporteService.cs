using Microsoft.EntityFrameworkCore;
using SuperBodega.Admin.API.Data;
using SuperBodega.Admin.API.Dtos;
using SuperBodega.Admin.API.Services.Interfaces;

namespace SuperBodega.Admin.API.Services
{
    public class ReporteService : IReporteService
    {
        private readonly SuperBodegaDbContext _context;

        public ReporteService(SuperBodegaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReporteVentaDto>> ObtenerPorClienteAsync(int clienteId)
        {
            return await _context.Ventas
                .Where(v => v.ClienteId == clienteId)
                .Select(v => new ReporteVentaDto
                {
                    VentaId = v.Id,
                    Fecha = v.Fecha ?? DateTime.MinValue,
                    Cliente = v.Cliente!.Nombre,
                    Estado = v.Estado!,
                    Total = v.Total ?? 0
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ReporteVentaDto>> ObtenerPorProductoAsync(int productoId)
        {
            return await _context.DetalleVenta
                .Where(d => d.ProductoId == productoId)
                .Select(d => new ReporteVentaDto
                {
                    VentaId = d.Venta.Id,
                    Fecha = d.Venta.Fecha ?? DateTime.MinValue,
                    Cliente = d.Venta.Cliente!.Nombre,
                    Estado = d.Venta.Estado!,
                    Total = d.Venta.Total ?? 0
                })
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<ReporteVentaDto>> ObtenerPorProveedorAsync(int proveedorId)
        {
            return await _context.Ventas
                .Where(v => v.DetalleVenta.Any(d => d.Producto!.ProveedorId == proveedorId))
                .Select(v => new ReporteVentaDto
                {
                    VentaId = v.Id,
                    Fecha = v.Fecha ?? DateTime.MinValue,
                    Cliente = v.Cliente!.Nombre,
                    Estado = v.Estado!,
                    Total = v.Total ?? 0
                })
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<ReporteVentaDto>> ObtenerPorFechaAsync(DateTime inicio, DateTime fin)
        {
            return await _context.Ventas
                .Where(v => v.Fecha >= inicio && v.Fecha <= fin)
                .Select(v => new ReporteVentaDto
                {
                    VentaId = v.Id,
                    Fecha = v.Fecha ?? DateTime.MinValue,
                    Cliente = v.Cliente!.Nombre,
                    Estado = v.Estado!,
                    Total = v.Total ?? 0
                })
                .ToListAsync();
        }
    }
}

