using Xunit;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using SuperBodega.Admin.API.Data;
using SuperBodega.Admin.API.Services;
using SuperBodega.Admin.API.Dtos;
using SuperBodega.Admin.API.Models;
using SuperBodega.Models;

public class CompraServiceTests
{
    private readonly IMapper _mapper;

    public CompraServiceTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Compra, CompraDto>();
            cfg.CreateMap<CompraCreateDto, Compra>();
            cfg.CreateMap<DetalleCompra, DetalleCompraDto>();
            cfg.CreateMap<DetalleCompraDto, DetalleCompra>();
        });
        _mapper = config.CreateMapper();
    }

    private SuperBodegaDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<SuperBodegaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // base limpia por test
            .Options;
        return new SuperBodegaDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_CreaCompra_Y_AumentaStock()
    {
        using var context = GetDbContext();

        // Agregamos un producto con stock inicial 10
        context.Productos.Add(new Producto { Id = 1, Nombre = "Producto 1", Stock = 10 });
        await context.SaveChangesAsync();

        var service = new CompraService(context, _mapper);

        var compraCreateDto = new CompraCreateDto
        {
            ProveedorId = 1,
            Detalle = new List<DetalleCompraDto>
            {
                new DetalleCompraDto { ProductoId = 1, Cantidad = 5, PrecioUnitario = 100 }
            }
        };

        var result = await service.CreateAsync(compraCreateDto);

        Assert.NotNull(result);
        Assert.Equal(1, result.ProveedorId);
        Assert.Single(result.Detalle);
        Assert.Equal(5, result.Detalle[0].Cantidad);

        var producto = await context.Productos.FindAsync(1);
        Assert.Equal(15, producto.Stock); // Stock aumentado en 5
    }

    [Fact]
    public async Task GetByIdAsync_DevuelveCompraConDetalle()
    {
        using var context = GetDbContext();

        var compra = new Compra { Id = 1, Fecha = DateTime.Now, ProveedorId = 1, Total = 500 };
        context.Compras.Add(compra);
        context.DetalleCompras.Add(new DetalleCompra { CompraId = 1, ProductoId = 1, Cantidad = 5, PrecioUnitario = 100 });
        await context.SaveChangesAsync();

        var service = new CompraService(context, _mapper);

        var result = await service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.NotNull(result.Detalle);
        Assert.Single(result.Detalle);
    }

    [Fact]
    public async Task DeleteAsync_EliminaCompraYDetalles()
    {
        using var context = GetDbContext();

        var compra = new Compra { Id = 1, Fecha = DateTime.Now, ProveedorId = 1, Total = 500 };
        context.Compras.Add(compra);
        context.DetalleCompras.Add(new DetalleCompra { CompraId = 1, ProductoId = 1, Cantidad = 5, PrecioUnitario = 100 });
        await context.SaveChangesAsync();

        var service = new CompraService(context, _mapper);

        var result = await service.DeleteAsync(1);

        Assert.True(result);
        Assert.Null(await context.Compras.FindAsync(1));
        Assert.Empty(context.DetalleCompras.Where(d => d.CompraId == 1));
    }
}

