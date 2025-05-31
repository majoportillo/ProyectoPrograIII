using ECommerce.API.Data;
using ECommerce.API.Dtos;
using ECommerce.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using SuperBodega.Admin.API.Dtos;
using SuperBodega.Models.Dtos;

namespace ECommerce.API.Services
{
    public class CarritoService : ICarritoService
    {
        private readonly ECommerceDbContext _context;

        public CarritoService(ECommerceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CarritoDto>> ObtenerPorClienteAsync(int clienteId)
        {
            var carritos = await _context.Carritos
                .Include(c => c.Producto)
                .Where(c => c.ClienteId == clienteId)
                .ToListAsync();

            return carritos.Select(c => new CarritoDto
            {
                Id = c.Id,
                ClienteId = c.ClienteId ?? 0,
                ProductoId = c.ProductoId ?? 0,
                Cantidad = c.Cantidad,
                FechaAgregado = c.FechaAgregado,
                Producto = c.Producto == null ? null : new ProductoDto
                {
                    Id = c.Producto.Id,
                    Nombre = c.Producto.Nombre,
                    Categoria = c.Producto.Categoria,
                    Precio = c.Producto.Precio,
                    Descripcion = c.Producto.Descripcion
                }
            });
        }

        public async Task<CarritoDto> AgregarAsync(CarritoDto dto)
        {
            // Buscar si ya existe el producto en el carrito del cliente
            var existente = await _context.Carritos
                .FirstOrDefaultAsync(c => c.ClienteId == dto.ClienteId && c.ProductoId == dto.ProductoId);

            if (existente != null)
            {
                // Si ya existe, aumentar la cantidad
                existente.Cantidad += dto.Cantidad;
                await _context.SaveChangesAsync();

                return new CarritoDto
                {
                    Id = existente.Id,
                    ClienteId = existente.ClienteId ?? 0,
                    ProductoId = existente.ProductoId ?? 0,
                    Cantidad = existente.Cantidad,
                    FechaAgregado = existente.FechaAgregado
                };
            }

            // Si no existe, crear uno nuevo
            var nuevo = new SuperBodega.Models.Carrito
            {
                ClienteId = dto.ClienteId,
                ProductoId = dto.ProductoId,
                Cantidad = dto.Cantidad,
                FechaAgregado = DateTime.Now
            };

            _context.Carritos.Add(nuevo);
            await _context.SaveChangesAsync();

            return new CarritoDto
            {
                Id = nuevo.Id,
                ClienteId = nuevo.ClienteId ?? 0,
                ProductoId = nuevo.ProductoId ?? 0,
                Cantidad = nuevo.Cantidad,
                FechaAgregado = nuevo.FechaAgregado
            };
        }


        public async Task<bool> EliminarAsync(int id)
        {
            var item = await _context.Carritos.FindAsync(id);
            if (item == null) return false;

            _context.Carritos.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> VaciarAsync(int clienteId)
        {
            var items = await _context.Carritos
                .Where(c => c.ClienteId == clienteId)
                .ToListAsync();

            if (!items.Any()) return false;

            _context.Carritos.RemoveRange(items);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

