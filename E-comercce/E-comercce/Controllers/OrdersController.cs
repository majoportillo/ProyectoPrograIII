using E_comercce_.Data;
using E_comercce_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_comercce_.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public OrdersController(ApplicationDbContext db) => _db = db;

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] Order order)
        {
            order.CreatedAt = DateTime.UtcNow;
            order.Status = "Recibido";

            _db.Orders.Add(order);

            // Vaciar carrito para ese cliente
            var cartItems = _db.CartItems.Where(ci => ci.CustomerId == order.CustomerId);
            _db.CartItems.RemoveRange(cartItems);

            await _db.SaveChangesAsync();
            return Ok(new { message = "Pedido realizado con éxito", orderId = order.Id });
        }

        [HttpGet("{customerId:int}")]
        public async Task<IActionResult> GetOrdersByCustomer(int customerId)
        {
            var orders = await _db.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.CustomerId == customerId)
                .ToListAsync();
            return Ok(orders);
        }
    }
}
