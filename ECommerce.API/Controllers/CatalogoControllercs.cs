using Microsoft.AspNetCore.Mvc;
using ECommerce.API.Services.Interfaces;
using SuperBodega.Models.Dtos;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogoController : ControllerBase
    {
        private readonly IProductoCatalogoService _service;

        public CatalogoController(IProductoCatalogoService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CatalogoProductoDto>>> Get(
            string? filtro = null,
            string tipoFiltro = "nombre",
            int pagina = 1,
            int tamanoPagina = 10)
        {
            var productos = await _service.ObtenerCatalogoAsync(filtro ?? "", tipoFiltro, pagina, tamanoPagina);
            return Ok(productos);
        }

    }
}

