using E_comercce_.Data;
using E_comercce_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_comercce_.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public CartController(ApplicationDbContext db) => _db = db;

        [HttpGet("{customerId:int}")]
        public async Task<IActionResult> GetCartItems(int customerId)
        {
            var items = await _db.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.CustomerId == customerId)
                .ToListAsync();
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] CartItem newItem)
        {
            var existing = await _db.CartItems
                .FirstOrDefaultAsync(ci => ci.CustomerId == newItem.CustomerId && ci.ProductId == newItem.ProductId);

            if (existing != null)
            {
                existing.Quantity += newItem.Quantity;
            }
            else
            {
                _db.CartItems.Add(newItem);
            }

            await _db.SaveChangesAsync();
            return Ok(new { message = "Producto agregado al carrito" });
        }

        [HttpDelete("{cartItemId:int}")]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            var item = await _db.CartItems.FindAsync(cartItemId);
            if (item == null) return NotFound();
            _db.CartItems.Remove(item);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Producto removido del carrito" });
        }
    }
}
