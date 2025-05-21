using E_commerce.Models;
using ECommerce.Api.Data;
using ECommerce.Api.Models;
using global::ECommerce.Api.Data;
using global::ECommerce.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ECommerceDbContext _context;

        public CartController(ECommerceDbContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] CartItem item, [FromQuery] string customerEmail)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.CustomerEmail == customerEmail);

            if (cart == null)
            {
                cart = new Cart { CustomerEmail = customerEmail, Items = new List<CartItem>() }; // Ensure Items is initialized.
                _context.Carts.Add(cart);
            }

            item.Product = await _context.Products.FindAsync(item.ProductId);
            if (item.Product == null) return NotFound("Producto no encontrado");

            cart.Items.Add(item); // Ensure the CartItem type matches the one used in the Cart.Items list.
            await _context.SaveChangesAsync();

            return Ok(cart);
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetCart(string email)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.CustomerEmail == email);

            return cart == null ? NotFound() : Ok(cart);
        }
    }
}
