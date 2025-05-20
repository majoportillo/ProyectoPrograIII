using SuperBodega.Admin.API.Dtos;

namespace SuperBodega.Admin.API.Services.Interfaces
{
    public interface IProductoService
    {
        Task<IEnumerable<ProductoDto>> GetAllAsync();
        Task<ProductoDto?> GetByIdAsync(int id);
        Task<ProductoDto> CreateAsync(ProductoDto dto);
        Task<bool> UpdateAsync(int id, ProductoDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
