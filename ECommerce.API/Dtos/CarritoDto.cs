
using SuperBodega.Admin.API.Dtos;
using SuperBodega.Models.Dtos;

namespace ECommerce.API.Dtos
{
    public class CarritoDto
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public DateTime? FechaAgregado { get; set; }
        public ProductoDto? Producto { get; set; }

    }
}
