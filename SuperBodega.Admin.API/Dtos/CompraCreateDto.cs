namespace SuperBodega.Admin.API.Dtos
{
    public class CompraCreateDto
    {
        public int? ProveedorId { get; set; }
        public List<DetalleCompraDto> Detalle { get; set; } = new();
    }
}
