using ECommerce.API.Dtos;
using ECommerce.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SuperBodega.Models.Dtos;

namespace ECommerce.API.Controllers
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
            var items = await _carritoService.ObtenerPorClienteAsync(clienteId);
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<CarritoDto>> Post(CarritoDto dto)
        {
            var creado = await _carritoService.AgregarAsync(dto);
            return CreatedAtAction(nameof(Get), new { clienteId = dto.ClienteId }, creado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _carritoService.EliminarAsync(id);
            return eliminado ? NoContent() : NotFound();
        }

        [HttpDelete("vaciar/{clienteId}")]
        public async Task<IActionResult> VaciarAsync(int clienteId)
        {
            var eliminado = await _carritoService.VaciarAsync(clienteId);
            return eliminado ? NoContent() : NotFound();
        }
    }
}



