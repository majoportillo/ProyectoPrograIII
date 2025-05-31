using System.Net.Http;
using System.Text.Json;
using SuperBodega.ECommerce.API.Dtos;

namespace SuperBodega.ECommerce.API.Services
{
    public class ProductoService
    {
        private readonly HttpClient _httpClient;

        public ProductoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductoDto>> GetProductosAsync()
        {
            var response = await _httpClient.GetAsync("http://localhost:5000/api/productos"); // Cambia la URL al puerto real de la API principal
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var productos = JsonSerializer.Deserialize<List<ProductoDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return productos ?? new List<ProductoDto>();
        }
    }
}