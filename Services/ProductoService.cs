using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoFinalPrograIII.DTOs;
using ProyectoFinalPrograIII.Services.Interfaces;

namespace ProyectoFinalPrograIII.Services
{
    public class ProductoService :IProductoService
    {
        public ProductoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductoDto>> GetAllAsync()
        {
            return await _context.Productos
                .Select(p => new ProductoDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    Stock = p.Stock,
                    Categoria = p.Categoria,
                    ProveedorId = p.ProveedorId
                })
                .ToListAsync();
        }

        public async Task<ProductoDto?> GetByIdAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return null;

            return new ProductoDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Stock = producto.Stock,
                Categoria = producto.Categoria,
                ProveedorId = producto.ProveedorId
            };
        }

    }
}
