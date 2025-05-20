using Microsoft.AspNetCore.Mvc;
using SuperBodega.Admin.API.Dtos;
using SuperBodega.Admin.API.Services.Interfaces;

namespace SuperBodega.Admin.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesController : ControllerBase
    {
        private readonly IReporteService _reporteService;

        public ReportesController(IReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        [HttpGet("ventas-por-cliente/{id}")]
        public async Task<ActionResult<IEnumerable<ReporteVentaDto>>> VentasPorCliente(int id)
        {
            return Ok(await _reporteService.ObtenerPorClienteAsync(id));
        }

        [HttpGet("ventas-por-producto/{id}")]
        public async Task<ActionResult<IEnumerable<ReporteVentaDto>>> VentasPorProducto(int id)
        {
            return Ok(await _reporteService.ObtenerPorProductoAsync(id));
        }

        [HttpGet("ventas-por-proveedor/{id}")]
        public async Task<ActionResult<IEnumerable<ReporteVentaDto>>> VentasPorProveedor(int id)
        {
            return Ok(await _reporteService.ObtenerPorProveedorAsync(id));
        }

        [HttpGet("ventas-por-fecha")]
        public async Task<ActionResult<IEnumerable<ReporteVentaDto>>> VentasPorFecha([FromQuery] DateTime inicio, [FromQuery] DateTime fin)
        {
            return Ok(await _reporteService.ObtenerPorFechaAsync(inicio, fin));
        }
    }
}

