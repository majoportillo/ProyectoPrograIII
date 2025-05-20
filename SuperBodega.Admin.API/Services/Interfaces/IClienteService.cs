using SuperBodega.Admin.API.Dtos;

namespace SuperBodega.Admin.API.Services.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteDto>> GetAllAsync();
        Task<ClienteDto?> GetByIdAsync(int id);
        Task<ClienteDto> CreateAsync(ClienteDto dto);
        Task<bool> UpdateAsync(int id, ClienteDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
