using Microsoft.AspNetCore.Mvc;
using SuperBodega.Admin.API.Dtos;
using SuperBodega.Admin.API.Services.Interfaces;

namespace SuperBodega.Admin.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedoresController : ControllerBase
    {
        private readonly IProveedorService _proveedorService;

        public ProveedoresController(IProveedorService proveedorService)
        {
            _proveedorService = proveedorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProveedorDto>>> Get()
        {
            return Ok(await _proveedorService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProveedorDto>> Get(int id)
        {
            var proveedor = await _proveedorService.GetByIdAsync(id);
            return proveedor == null ? NotFound() : Ok(proveedor);
        }

        [HttpPost]
        public async Task<ActionResult<ProveedorDto>> Post(ProveedorDto dto)
        {
            var creado = await _proveedorService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = creado.Id }, creado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ProveedorDto dto)
        {
            var actualizado = await _proveedorService.UpdateAsync(id, dto);
            return actualizado ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _proveedorService.DeleteAsync(id);
            return eliminado ? NoContent() : NotFound();
        }
    }
}

