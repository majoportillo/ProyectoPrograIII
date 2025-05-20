using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SuperBodega.Admin.API.Data;
using SuperBodega.Admin.API.Dtos;
using SuperBodega.Admin.API.Models;
using SuperBodega.Admin.API.Services.Interfaces;
using SuperBodega.Models;

namespace SuperBodega.Admin.API.Services
{
    public class CompraService : ICompraService
    {
        private readonly SuperBodegaDbContext _context;
        private readonly IMapper _mapper;

        public CompraService(SuperBodegaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CompraDto>> GetAllAsync()
        {
            var compras = await _context.Compras
                .Include(c => c.DetalleCompras)
                .ToListAsync();

            var result = new List<CompraDto>();
            foreach (var compra in compras)
            {
                var dto = _mapper.Map<CompraDto>(compra);
                dto.Detalle = _mapper.Map<List<DetalleCompraDto>>(compra.DetalleCompras);
                result.Add(dto);
            }

            return result;
        }

        public async Task<CompraDto?> GetByIdAsync(int id)
        {
            var compra = await _context.Compras
                .Include(c => c.DetalleCompras)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (compra == null) return null;

            var dto = _mapper.Map<CompraDto>(compra);
            dto.Detalle = _mapper.Map<List<DetalleCompraDto>>(compra.DetalleCompras);
            return dto;
        }

        public async Task<CompraDto> CreateAsync(CompraCreateDto dto)
        {
            var compra = new Compra
            {
                Fecha = DateTime.Now,
                ProveedorId = dto.ProveedorId,
                Total = dto.Detalle.Sum(d => d.PrecioUnitario * d.Cantidad)
            };

            _context.Compras.Add(compra);
            await _context.SaveChangesAsync(); // para obtener ID

            foreach (var detalle in dto.Detalle)
            {
                var detalleCompra = new DetalleCompra
                {
                    CompraId = compra.Id,
                    ProductoId = detalle.ProductoId,
                    Cantidad = detalle.Cantidad,
                    PrecioUnitario = detalle.PrecioUnitario
                };

                _context.DetalleCompras.Add(detalleCompra);

                // ✅ Aumentar stock del producto
                var producto = await _context.Productos.FindAsync(detalle.ProductoId);
                if (producto != null)
                {
                    producto.Stock = (producto.Stock ?? 0) + detalle.Cantidad;
                }
            }

            await _context.SaveChangesAsync();

            // Devolver DTO
            var result = _mapper.Map<CompraDto>(compra);
            result.Detalle = dto.Detalle;
            return result;
        }
    }
}

