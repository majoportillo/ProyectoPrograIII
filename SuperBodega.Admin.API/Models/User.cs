namespace SuperBodega.Admin.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UsuarioNombre { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = default!;
        public byte[] PasswordSalt { get; set; } = default!;
    }
}
