using Ecomerce.Models;
using ECommerce.Api.Data;
using ECommerce.Api.Models;
using ECommerce.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ECommerceDbContext _context;
        private readonly OrderStatusPublisher _publisher;

        public OrdersController(ECommerceDbContext context, OrderStatusPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
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
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            // Publicar mensaje a la cola para enviar notificación email
            _publisher.PublishOrderStatus(customerEmail, order.Status);

            return Ok(order);
        }
    }
}
