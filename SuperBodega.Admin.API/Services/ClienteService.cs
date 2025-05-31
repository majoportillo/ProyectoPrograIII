using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SuperBodega.Admin.API.Data;
using SuperBodega.Admin.API.Dtos;
using SuperBodega.Admin.API.Models;
using SuperBodega.Admin.API.Services.Interfaces;

namespace SuperBodega.Admin.API.Services
{
    public class ClienteService : IClienteService
    {
        private readonly SuperBodegaDbContext _context;
        private readonly IMapper _mapper;

        public ClienteService(SuperBodegaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClienteDto>> GetAllAsync()
        {
            var clientes = await _context.Clientes.ToListAsync();
            return _mapper.Map<List<ClienteDto>>(clientes);
        }

        public async Task<ClienteDto?> GetByIdAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            return cliente == null ? null : _mapper.Map<ClienteDto>(cliente);
        }

        public async Task<ClienteDto> CreateAsync(ClienteDto dto)
        {
            var cliente = _mapper.Map<Cliente>(dto);
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return _mapper.Map<ClienteDto>(cliente);
        }

        public async Task<bool> UpdateAsync(int id, ClienteDto dto)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return false;

            _mapper.Map(dto, cliente);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return false;

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
