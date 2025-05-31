using AutoMapper;
using SuperBodega.Admin.API.Dtos;
using SuperBodega.Admin.API.Models;
using SuperBodega.Models;

namespace SuperBodega.Admin.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Producto
            CreateMap<Producto, ProductoDto>().ReverseMap();

            // Cliente
            CreateMap<Cliente, ClienteDto>().ReverseMap();

            // Proveedor
            CreateMap<Proveedores, ProveedorDto>().ReverseMap();

            // Compra y DetalleCompra
            CreateMap<Compra, CompraDto>().ReverseMap();
            CreateMap<DetalleCompra, DetalleCompraDto>().ReverseMap();

            // Venta y DetalleVenta
            CreateMap<Venta, VentaDto>().ReverseMap();
            CreateMap<DetalleVenta, DetalleVentaDto>().ReverseMap();

            // Carrito
            CreateMap<Carrito, CarritoDto>().ReverseMap();
        }
    }
}

