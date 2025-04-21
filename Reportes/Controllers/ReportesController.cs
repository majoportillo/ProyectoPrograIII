using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SuperBodega.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesController : ControllerBase
    {
        private readonly SuperBodegaContext _context;

        public ReportesController(SuperBodegaContext context)
        {
            _context = context;
        }

        //Reporte de ventas por periodo de tiempo
        [HttpGet("ventas-periodo")]
        public async Task<IActionResult> GetVentasPorPeriodo(DateTime fechaInicio, DateTime fechaFin)
        {
            var ventas = await _context.Ventas
                .Where(v => v.Fecha >= fechaInicio && v.Fecha <= fechaFin)
                .ToListAsync();

            if (ventas == null || !ventas.Any())
            {
                return NotFound("No se encontraron ventas en el periodo especificado.");
            }

            return Ok(ventas);
        }

        //Reporte de ventas por producto
        [HttpGet("ventas-producto")]
        public async Task<IActionResult> GetVentasPorProducto()
        {
            var ventas = await _context.Ventas
                .SelectMany(v => v.Detalles)
                .GroupBy(d => d.Producto.Nombre)
                .Select(g => new
                {
                    Producto = g.Key,
                    TotalVendido = g.Sum(x => x.Cantidad)
                })
                .ToListAsync();

            if (ventas == null || !ventas.Any())
            {
                return NotFound("No se encontraron ventas para el producto especificado.");
            }

            return Ok(ventas);
        }

        //Reporte de ventas por cliente
        [HttpGet("ventas-cliente")]
        public async Task<IActionResult> GetVentasPorCliente()
        {
            var ventas = await _context.Ventas
                .GroupBy(v => v.Cliente.Nombre)
                .Select(g => new
                {
                    Cliente = g.Key,
                    TotalVentas = g.Count(),
                    TotalMonto = g.SelectMany(v => v.Detalles).Sum(d => d.Precio * d.Cantidad)
                })
                .ToListAsync();

            if (ventas == null || !ventas.Any())
            {
                return NotFound("No se encontraron ventas para el cliente especificado.");
            }

            return Ok(ventas);
        }

        //Reporte de ventas por proveedor
        [HttpGet("ventas-proveedor")]
        public async Task<IActionResult> GetVentasPorProveedor()
        {
            var ventas = await _context.Ventas
                .GroupBy(v => v.Proveedor.Nombre)
                .Select(g => new
                {
                    Proveedor = g.Key,
                    TotalVentas = g.Count(),
                    TotalMonto = g.SelectMany(v => v.Detalles).Sum(d => d.Precio * d.Cantidad)
                })
                .ToListAsync();

            if (ventas == null || !ventas.Any())
            {
                return NotFound("No se encontraron ventas para el proveedor especificado.");
            }

            return Ok(ventas);
    }
}