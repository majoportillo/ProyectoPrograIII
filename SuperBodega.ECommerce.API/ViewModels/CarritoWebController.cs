using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

public class CarritoWebController : Controller
{
    private readonly IHttpClientFactory _clientFactory;

    public CarritoWebController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    [HttpPost]
    public async Task<IActionResult> Agregar(int productoId, int cantidad)
    {
        var client = _clientFactory.CreateClient();
        var payload = new { ProductoId = productoId, Cantidad = cantidad };

        // Cambia el puerto a donde realmente está tu API
        var response = await client.PostAsJsonAsync("https://localhost:7032/api/carrito", payload);

        if (response.IsSuccessStatusCode)
        {
            // Puedes redirigir a la página del carrito, o regresar a productos con mensaje
            TempData["Success"] = "Producto agregado al carrito.";
        }
        else
        {
            TempData["Error"] = "Error al agregar el producto al carrito.";
        }

        return RedirectToAction("Index", "ProductosWeb");
    }
}