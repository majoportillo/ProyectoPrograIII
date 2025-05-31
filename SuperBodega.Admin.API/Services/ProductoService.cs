using Microsoft.EntityFrameworkCore;
using SuperBodega.Admin.API.Data;
using SuperBodega.Admin.API.Dtos;
using SuperBodega.Models;
using SuperBodega.Admin.API.Services.Interfaces;

namespace SuperBodega.Admin.API.Services
{
    public class ProductoService : IProductoService
    {
        private readonly SuperBodegaDbContext _context;

        public ProductoService(SuperBodegaDbContext context)
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
                    Categoria = p.Categoria,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    Stock = p.Stock,
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
                Categoria = producto.Categoria,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Stock = producto.Stock,
                ProveedorId = producto.ProveedorId
            };
        }

        public async Task<ProductoDto> CreateAsync(ProductoDto dto)
        {
            var producto = new Producto
            {
                Nombre = dto.Nombre,
                Categoria = dto.Categoria,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                Stock = dto.Stock,
                ProveedorId = dto.ProveedorId
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            dto.Id = producto.Id; // devuelve el ID generado
            return dto;
        }

        public async Task<bool> UpdateAsync(int id, ProductoDto dto)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return false;

            producto.Nombre = dto.Nombre;
            producto.Categoria = dto.Categoria;
            producto.Descripcion = dto.Descripcion;
            producto.Precio = dto.Precio;
            producto.Stock = dto.Stock;
            producto.ProveedorId = dto.ProveedorId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return false;

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
