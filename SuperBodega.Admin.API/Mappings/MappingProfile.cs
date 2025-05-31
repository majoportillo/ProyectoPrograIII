using AutoMapper;
using SuperBodega.Admin.API.Dtos;
using SuperBodega.Models;

namespace SuperBodega.Admin.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Producto
            CreateMap<Producto, ProductoDto>().ReverseMap();
            CreateMap<ProductoDto, Producto>()   // escritura (sin tocar Id)
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Cliente
            CreateMap<Cliente, ClienteDto>();
            CreateMap<ClienteDto, Cliente>()   // escritura (sin tocar Id)
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Proveedor
            CreateMap<Proveedores, ProveedorDto>();
            CreateMap<ProveedorDto, Proveedores>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Compra y DetalleCompra
            CreateMap<Compra, CompraDto>().ReverseMap();
            CreateMap<DetalleCompra, DetalleCompraDto>().ReverseMap();

            // Venta y DetalleVenta
            CreateMap<Venta, VentaDto>().ReverseMap();
            CreateMap<DetalleVenta, DetalleVentaDto>().ReverseMap();
        }
    }
}

