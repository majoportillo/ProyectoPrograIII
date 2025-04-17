namespace SuperBodega.API.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public string Categoria { get; set; }

        public string Descripcion { get; set; }
        public string ImagenUrl { get; set; }
        public int Disponibilidad { get; set; }
    }
}