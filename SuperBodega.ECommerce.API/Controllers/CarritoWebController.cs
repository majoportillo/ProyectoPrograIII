using Microsoft.AspNetCore.Mvc;
using SuperBodega.ECommerce.API.Dtos;

namespace SuperBodega.ECommerce.API.Controllers
{
    public class CarritoWebController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public CarritoWebController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        

        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient();

            var carrito = await client.GetFromJsonAsync<CarritoDto>("https://localhost:7032/api/carrito");

            return View(carrito ?? new CarritoDto());
        }
    }
}
