using ECommerce.API.Dtos;
using SuperBodega.Models.Dtos;

namespace ECommerce.API.Services.Interfaces
{
    public interface ICarritoService
    {
        Task<IEnumerable<CarritoDto>> ObtenerPorClienteAsync(int clienteId);
        Task<CarritoDto> AgregarAsync(CarritoDto dto);
        Task<bool> EliminarAsync(int id);
        Task<bool> VaciarAsync(int clienteId); // opcional
    }
}
