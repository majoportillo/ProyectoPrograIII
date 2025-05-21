using E_commerce.Models;
using ECommerce.Api.Models;

namespace Ecomerce.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerEmail { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public List<CartItem> Items { get; set; } = new();
        public string Status { get; set; } = "Pedido recibido"; // Estado inicial
    }
}
