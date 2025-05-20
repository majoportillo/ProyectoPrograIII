using Microsoft.AspNetCore.Mvc;
using SuperBodega.Admin.API.Dtos;
using SuperBodega.Admin.API.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productoService;

    public ProductosController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductoDto>>> Get()
    {
        return Ok(await _productoService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductoDto>> Get(int id)
    {
        var producto = await _productoService.GetByIdAsync(id);
        return producto == null ? NotFound() : Ok(producto);
    }

    [HttpPost]
    public async Task<ActionResult<ProductoDto>> Post(ProductoDto dto)
    {
        var created = await _productoService.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, ProductoDto dto)
    {
        var updated = await _productoService.UpdateAsync(id, dto);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _productoService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
