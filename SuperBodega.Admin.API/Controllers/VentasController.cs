using Microsoft.AspNetCore.Mvc;
using SuperBodega.Admin.API.Dtos;
using SuperBodega.Admin.API.Services.Interfaces;

namespace SuperBodega.Admin.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentasController : ControllerBase
    {
        private readonly IVentaService _ventaService;

        public VentasController(IVentaService ventaService)
        {
            _ventaService = ventaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VentaDto>>> Get()
        {
            return Ok(await _ventaService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VentaDto>> Get(int id)
        {
            var venta = await _ventaService.GetByIdAsync(id);
            return venta == null ? NotFound() : Ok(venta);
        }

        [HttpPost]
        public async Task<ActionResult<VentaDto>> Post(VentaCreateDto dto)
        {
            var creada = await _ventaService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = creada.Id }, creada);
        }

        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] string estado)
        {
            var actualizado = await _ventaService.CambiarEstadoAsync(id, estado);
            return actualizado ? NoContent() : NotFound();
        }
    }
}

