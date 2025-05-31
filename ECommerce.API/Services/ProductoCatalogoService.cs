using ECommerce.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using ECommerce.API.Data;
using SuperBodega.Models.Dtos;

namespace ECommerce.API.Services
{
    public class ProductoCatalogoService : IProductoCatalogoService
    {
        private readonly ECommerceDbContext _context;

        public ProductoCatalogoService(ECommerceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CatalogoProductoDto>> ObtenerCatalogoAsync(string filtro, string tipoFiltro, int pagina, int tamanoPagina)
        {
            var query = _context.Productos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                switch (tipoFiltro.ToLower())
                {
                    case "categoria":
                        query = query.Where(p => p.Categoria!.ToLower().Contains(filtro.ToLower()));
                        break;
                    case "descripcion":
                        query = query.Where(p => p.Descripcion!.ToLower().Contains(filtro.ToLower()));
                        break;
                    case "nombre":
                    default:
                        query = query.Where(p => p.Nombre!.ToLower().Contains(filtro.ToLower()));
                        break;
                }
            }

            var productos = await query
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync();

            return productos.Select(p => new CatalogoProductoDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Categoria = p.Categoria,
                Descripcion = p.Descripcion,
                Precio = p.Precio
            });
        }


    }
}

