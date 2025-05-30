using Microsoft.AspNetCore.Mvc;
using SuperBodega.Admin.API.Dtos;
using SuperBodega.Admin.API.Services.Interfaces;

namespace SuperBodega.Admin.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComprasController : ControllerBase
    {
        private readonly ICompraService _compraService;

        public ComprasController(ICompraService compraService)
        {
            _compraService = compraService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompraDto>>> Get()
        {
            return Ok(await _compraService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompraDto>> Get(int id)
        {
            var compra = await _compraService.GetByIdAsync(id);
            return compra == null ? NotFound() : Ok(compra);
        }

        [HttpPost]
        public async Task<ActionResult<CompraDto>> Post(CompraCreateDto dto)
        {
            var creada = await _compraService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = creada.Id }, creada);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var compra = await _compraService.GetByIdAsync(id);
            if (compra == null)
                return NotFound();

            var eliminado = await _compraService.DeleteAsync(id);
            if (!eliminado)
                return StatusCode(500, "Error al eliminar la compra.");

            return NoContent();
        }


    }
}

