namespace SuperBodega.Admin.API.Dtos
{
    public class ReporteVentaDto
    {
        public int VentaId { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public decimal Total { get; set; }
    }
}

