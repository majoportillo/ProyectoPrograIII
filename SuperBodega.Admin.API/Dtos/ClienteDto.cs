namespace SuperBodega.Admin.API.Dtos
{
    public class ClienteDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Direccion { get; set; }
        public string? Email { get; set; }
    }
}
