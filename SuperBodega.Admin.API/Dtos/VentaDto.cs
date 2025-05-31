namespace SuperBodega.Admin.API.Dtos
{
    public class VentaDto
    {
        public int Id { get; set; }
        public DateTime? Fecha { get; set; }
        public decimal? Total { get; set; }
        public string? Estado { get; set; }
        public int? ClienteId { get; set; }
        public List<DetalleVentaDto> Detalle { get; set; } = new();
    }
}

