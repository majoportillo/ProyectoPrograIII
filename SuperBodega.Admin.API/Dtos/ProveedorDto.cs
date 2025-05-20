namespace SuperBodega.Admin.API.Dtos
{
    public class ProveedorDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
    }
}
