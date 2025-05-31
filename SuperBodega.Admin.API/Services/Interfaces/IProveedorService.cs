using SuperBodega.Admin.API.Dtos;

namespace SuperBodega.Admin.API.Services.Interfaces
{
    public interface IProveedorService
    {
        Task<IEnumerable<ProveedorDto>> GetAllAsync();
        Task<ProveedorDto?> GetByIdAsync(int id);
        Task<ProveedorDto> CreateAsync(ProveedorDto dto);
        Task<bool> UpdateAsync(int id, ProveedorDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
