using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SuperBodega.Admin.API.Data;
using SuperBodega.Admin.API.Dtos;
using SuperBodega.Models;
using SuperBodega.Admin.API.Services.Interfaces;

namespace SuperBodega.Admin.API.Services
{
    public class ProveedorService : IProveedorService
    {
        private readonly SuperBodegaDbContext _context;
        private readonly IMapper _mapper;

        public ProveedorService(SuperBodegaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProveedorDto>> GetAllAsync()
        {
            var proveedores = await _context.Proveedores.ToListAsync();
            return _mapper.Map<List<ProveedorDto>>(proveedores);
        }

        public async Task<ProveedorDto?> GetByIdAsync(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            return proveedor == null ? null : _mapper.Map<ProveedorDto>(proveedor);
        }

        public async Task<ProveedorDto> CreateAsync(ProveedorDto dto)
        {
            var proveedor = _mapper.Map<Proveedores>(dto);
            _context.Proveedores.Add(proveedor);
            await _context.SaveChangesAsync();
            return _mapper.Map<ProveedorDto>(proveedor);
        }

        public async Task<bool> UpdateAsync(int id, ProveedorDto dto)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null) return false;

            _mapper.Map(dto, proveedor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null) return false;

            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

