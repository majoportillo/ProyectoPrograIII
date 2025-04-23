public class Venta
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public int Cliente Cliente { get; set; }
    public int Proveedor Proveedor { get; set; }
    public List<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
}