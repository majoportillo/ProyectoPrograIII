namespace SuperBodega.ECommerce.API.Dtos
{
    public class CarritoDto
    {
        public List<ItemCarritoDto> Items { get; set; } = new List<ItemCarritoDto>();
        public decimal Total { get; set; }
    }

    public class ItemCarritoDto
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal => Precio * Cantidad;
    }
}
