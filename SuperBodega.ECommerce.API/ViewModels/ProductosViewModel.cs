using SuperBodega.ECommerce.API.Dtos;

namespace SuperBodega.ECommerce.API.ViewModels
{
    public class ProductosViewModel
    {
        public IEnumerable<ProductoDto> Productos { get; set; } = new List<ProductoDto>();
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
        public int? CategoriaId { get; set; }
        public string? Search { get; set; }
        // Puedes agregar más propiedades después (lista de categorías, etc.)
    }
}
