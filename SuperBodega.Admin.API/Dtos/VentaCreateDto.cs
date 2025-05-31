namespace SuperBodega.Admin.API.Dtos
{
    public class VentaCreateDto
    {
        public int? ClienteId { get; set; }
        public List<DetalleVentaDto> Detalle { get; set; } = new();
    }
}

