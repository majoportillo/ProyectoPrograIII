using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using SuperBodega.ECommerce.API.ViewModels;
using SuperBodega.ECommerce.API.Dtos;

public class ProductosWebController : Controller
{
    private readonly IHttpClientFactory _clientFactory;

    public ProductosWebController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<IActionResult> Index(int page = 1, int? categoriaId = null, string? search = null)
    {
        var url = $"https://localhost:7032/api/productos?page={page}&pageSize=20";
        if (categoriaId.HasValue)
            url += $"&categoriaId={categoriaId.Value}";
        if (!string.IsNullOrEmpty(search))
            url += $"&search={Uri.EscapeDataString(search)}";

        //Productos de prueba
        var productos = new List<ProductoDto>
    {
        new ProductoDto { Id = 1, Nombre = "Leche Entera", Precio = 10.5M, Categoria = "Lácteos" },
        new ProductoDto { Id = 2, Nombre = "Pan Integral", Precio = 4.25M, Categoria = "Panadería" },
        new ProductoDto { Id = 3, Nombre = "Huevos Docena", Precio = 12.0M, Categoria = "Huevos" },
        new ProductoDto { Id = 4, Nombre = "Queso Mozzarella", Precio = 20.0M, Categoria = "Lácteos" },
        new ProductoDto { Id = 5, Nombre = "Yogurt Natural", Precio = 7.5M, Categoria = "Lácteos" },
        new ProductoDto { Id = 6, Nombre = "Mantequilla", Precio = 8.75M, Categoria = "Lácteos" },
        new ProductoDto { Id = 7, Nombre = "Pan Francés", Precio = 5.0M, Categoria = "Panadería" },
        new ProductoDto { Id = 8, Nombre = "Croissant", Precio = 6.5M, Categoria = "Panadería" },
        new ProductoDto { Id = 9, Nombre = "Tortillas de Maíz", Precio = 3.0M, Categoria = "Panadería" },
        new ProductoDto { Id = 10, Nombre = "Jamonada de Pavo", Precio = 18.5M, Categoria = "Embutidos" },
        new ProductoDto { Id = 11, Nombre = "Salchicha Frankfurt", Precio = 16.0M, Categoria = "Embutidos" },
        new ProductoDto { Id = 12, Nombre = "Queso Cheddar", Precio = 21.0M, Categoria = "Lácteos" },
        new ProductoDto { Id = 13, Nombre = "Aceite de Oliva", Precio = 25.0M, Categoria = "Despensa" },
        new ProductoDto { Id = 14, Nombre = "Azúcar Morena", Precio = 4.0M, Categoria = "Despensa" },
        new ProductoDto { Id = 15, Nombre = "Café Molido", Precio = 15.0M, Categoria = "Bebidas" },
        new ProductoDto { Id = 16, Nombre = "Té Verde", Precio = 8.0M, Categoria = "Bebidas" },
        new ProductoDto { Id = 17, Nombre = "Jugo de Naranja", Precio = 9.0M, Categoria = "Bebidas" },
        new ProductoDto { Id = 18, Nombre = "Agua Mineral", Precio = 2.5M, Categoria = "Bebidas" },
        new ProductoDto { Id = 19, Nombre = "Galletas de Chocolate", Precio = 7.25M, Categoria = "Snacks" },
        new ProductoDto { Id = 20, Nombre = "Papas Fritas", Precio = 6.0M, Categoria = "Snacks" }
    };
        var client = _clientFactory.CreateClient();
        //var result = await client.GetFromJsonAsync<PagedResult<ProductoDto>>(url);
        var vm = new ProductosViewModel
        {
            Productos = productos,
            PaginaActual = page,
            TotalPaginas = 1,
            CategoriaId = categoriaId,
            Search = search
        };
        /*var vm = new ProductosViewModel
        {
            Productos = result?.Items ?? new List<ProductoDto>(),
            PaginaActual = page,
            TotalPaginas = (int)Math.Ceiling((double)(result?.TotalItems ?? 0) / 20),
            CategoriaId = categoriaId,
            Search = search
        };*/
        return View(vm);
    }
}
