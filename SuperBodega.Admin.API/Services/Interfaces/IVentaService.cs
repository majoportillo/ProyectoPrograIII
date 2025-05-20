using SuperBodega.Admin.API.Dtos;

namespace SuperBodega.Admin.API.Services.Interfaces
{
    public interface IVentaService
    {
        Task<IEnumerable<VentaDto>> GetAllAsync();
        Task<VentaDto?> GetByIdAsync(int id);
        Task<VentaDto> CreateAsync(VentaCreateDto dto);
        Task<bool> CambiarEstadoAsync(int id, string nuevoEstado);
    }
}

