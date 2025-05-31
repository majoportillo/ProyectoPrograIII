using Microsoft.AspNetCore.Mvc;
using SuperBodega.Admin.API.Dtos;
using SuperBodega.Admin.API.Services.Interfaces;

namespace SuperBodega.Admin.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> Get()
        {
            return Ok(await _clienteService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDto>> Get(int id)
        {
            var cliente = await _clienteService.GetByIdAsync(id);
            return cliente == null ? NotFound() : Ok(cliente);
        }

        [HttpPost]
        public async Task<ActionResult<ClienteDto>> Post(ClienteDto dto)
        {
            var creado = await _clienteService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = creado.Id }, creado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ClienteDto dto)
        {
            var actualizado = await _clienteService.UpdateAsync(id, dto);
            return actualizado ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _clienteService.DeleteAsync(id);
            return eliminado ? NoContent() : NotFound();
        }
    }
}
