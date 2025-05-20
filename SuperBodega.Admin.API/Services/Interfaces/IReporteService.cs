using SuperBodega.Admin.API.Dtos;

namespace SuperBodega.Admin.API.Services.Interfaces
{
    public interface IReporteService
    {
        Task<IEnumerable<ReporteVentaDto>> ObtenerPorClienteAsync(int clienteId);
        Task<IEnumerable<ReporteVentaDto>> ObtenerPorProductoAsync(int productoId);
        Task<IEnumerable<ReporteVentaDto>> ObtenerPorProveedorAsync(int proveedorId);
        Task<IEnumerable<ReporteVentaDto>> ObtenerPorFechaAsync(DateTime inicio, DateTime fin);
    }
}

