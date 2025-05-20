using SuperBodega.Admin.API.Dtos;

namespace SuperBodega.Admin.API.Services.Interfaces
{
    public interface ICarritoService
    {
        Task<IEnumerable<CarritoDto>> ObtenerPorClienteAsync(int clienteId);
        Task<CarritoDto> AgregarAsync(CarritoDto dto);
        Task<bool> EliminarAsync(int id);
        Task<bool> VaciarCarritoAsync(int clienteId); // opcional
    }
}

