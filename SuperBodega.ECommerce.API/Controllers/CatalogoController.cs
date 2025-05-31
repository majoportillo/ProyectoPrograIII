using Microsoft.AspNetCore.Mvc;
using SuperBodega.ECommerce.API.Services;

namespace SuperBodega.ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogoController : ControllerBase
    {
        private readonly ProductoService _productoService;

        public CatalogoController(ProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var productos = await _productoService.GetProductosAsync();
            return Ok(productos);
        }
    }
}