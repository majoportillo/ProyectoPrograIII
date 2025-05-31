using SuperBodega.Admin.API.Dtos;

namespace SuperBodega.Admin.API.Services.Interfaces
{
    public interface ICompraService
    {
        Task<IEnumerable<CompraDto>> GetAllAsync();
        Task<CompraDto?> GetByIdAsync(int id);
        Task<CompraDto> CreateAsync(CompraCreateDto dto);
    }
}

