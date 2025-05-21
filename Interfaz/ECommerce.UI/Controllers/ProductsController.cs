using ECommerce.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace ECommerce.UI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public ProductsController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient("ECommerceApi");
            var response = await client.GetAsync("api/products");

            if (!response.IsSuccessStatusCode)
                return View("Error");

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return View(apiResponse.Products);
        }
    }

    public class ApiResponse
    {
        public int Total { get; set; }
        public List<Product> Products { get; set; }
    }
}
