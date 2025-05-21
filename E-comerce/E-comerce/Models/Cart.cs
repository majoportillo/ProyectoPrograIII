using E_commerce.Models;

namespace ECommerce.Api.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string CustomerEmail { get; set; } = string.Empty;
        public List<CartItem> Items { get; set; } = new();
    }
}