namespace SuperBodega.Admin.API.Dtos
{
    public class CompraDto
    {
        public int Id { get; set; }
        public DateTime? Fecha { get; set; }
        public int? ProveedorId { get; set; }
        public decimal? Total { get; set; }
        public List<DetalleCompraDto> Detalle { get; set; } = new();
    }
}
