namespace SuperBodega.Admin.API.Models
{
    public class User
    {
        public string usuario { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
