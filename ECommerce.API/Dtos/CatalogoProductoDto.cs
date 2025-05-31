namespace SuperBodega.Models.Dtos
{
    public class CatalogoProductoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public string? Categoria { get; set; }
        public string? Descripcion { get; set; }
    }
}

