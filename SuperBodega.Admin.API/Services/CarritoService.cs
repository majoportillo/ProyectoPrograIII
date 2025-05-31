using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SuperBodega.Admin.API.Data;
using SuperBodega.Admin.API.Dtos;
using SuperBodega.Admin.API.Models;
using SuperBodega.Admin.API.Services.Interfaces;
using SuperBodega.Models;

namespace SuperBodega.Admin.API.Services
{
    public class CarritoService : ICarritoService
    {
        private readonly SuperBodegaDbContext _context;
        private readonly IMapper _mapper;

        public CarritoService(SuperBodegaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CarritoDto>> ObtenerPorClienteAsync(int clienteId)
        {
            var items = await _context.Carritos
                .Where(c => c.ClienteId == clienteId)
                .ToListAsync();

            return _mapper.Map<List<CarritoDto>>(items);
        }

        public async Task<CarritoDto> AgregarAsync(CarritoDto dto)
        {
            var carrito = _mapper.Map<Carrito>(dto);
            carrito.FechaAgregado = DateTime.Now;

            _context.Carritos.Add(carrito);
            await _context.SaveChangesAsync();

            return _mapper.Map<CarritoDto>(carrito);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var item = await _context.Carritos.FindAsync(id);
            if (item == null) return false;

            _context.Carritos.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> VaciarCarritoAsync(int clienteId)
        {
            var items = await _context.Carritos.Where(c => c.ClienteId == clienteId).ToListAsync();
            if (!items.Any()) return false;

            _context.Carritos.RemoveRange(items);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
