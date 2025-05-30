using SuperBodega.Models.Dtos;

namespace ECommerce.API.Services.Interfaces
{
    public interface IProductoCatalogoService
    {
        Task<IEnumerable<CatalogoProductoDto>> ObtenerCatalogoAsync(string filtro, string tipoFiltro, int pagina, int tamanoPagina);

    }
}
