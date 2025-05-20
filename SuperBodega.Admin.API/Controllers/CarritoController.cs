using Microsoft.AspNetCore.Mvc;
using SuperBodega.Admin.API.Dtos;
using SuperBodega.Admin.API.Services.Interfaces;

namespace SuperBodega.Admin.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarritoController : ControllerBase
    {
        private readonly ICarritoService _carritoService;

        public CarritoController(ICarritoService carritoService)
        {
            _carritoService = carritoService;
        }

        [HttpGet("{clienteId}")]
        public async Task<ActionResult<IEnumerable<CarritoDto>>> Get(int clienteId)
        {
            return Ok(await _carritoService.ObtenerPorClienteAsync(clienteId));
        }

        [HttpPost]
        public async Task<ActionResult<CarritoDto>> Post(CarritoDto dto)
        {
            var creado = await _carritoService.AgregarAsync(dto);
            return CreatedAtAction(nameof(Get), new { clienteId = creado.ClienteId }, creado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _carritoService.EliminarAsync(id);
            return eliminado ? NoContent() : NotFound();
        }

        [HttpDelete("vaciar/{clienteId}")]
        public async Task<IActionResult> Vaciar(int clienteId)
        {
            var eliminado = await _carritoService.VaciarCarritoAsync(clienteId);
            return eliminado ? NoContent() : NotFound();
        }
    }
}

