using Ecomerce.Models;
using ECommerce.Api.Data;
using ECommerce.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ECommerceDbContext _context;

        public OrdersController(ECommerceDbContext context)
        {
            _context = context;
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromQuery] string customerEmail)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.CustomerEmail == customerEmail);

            if (cart == null || !cart.Items.Any())
                return BadRequest("Carrito vacío");

            var order = new Order
            {
                CustomerEmail = customerEmail,
                Items = cart.Items.ToList(),
                Status = "Pedido recibido"
            };

            _context.Orders.Add(order);
            _context.Carts.Remove(cart); // Limpia carrito
            await _context.SaveChangesAsync();

            return Ok(order);
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetOrders(string email)
        {
            var orders = await _context.Orders
                .Where(o => o.CustomerEmail == email)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .ToListAsync();

            return Ok(orders);
        }
    }
}
