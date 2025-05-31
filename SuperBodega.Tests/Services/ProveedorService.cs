using Xunit;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using SuperBodega.Admin.API.Data;
using SuperBodega.Admin.API.Services;
using SuperBodega.Admin.API.Dtos;
using SuperBodega.Models;

public class ProveedorServiceTests
{
    private readonly IMapper _mapper;

    public ProveedorServiceTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Proveedores, ProveedorDto>().ReverseMap();
        });
        _mapper = config.CreateMapper();
    }

    private SuperBodegaDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<SuperBodegaDbContext>()
            .UseInMemoryDatabase(databaseName: "ProveedorDbTest")
            .Options;
        return new SuperBodegaDbContext(options);
    }

    [Fact]
    public async Task GetAllAsync_DevuelveTodosLosProveedores()
    {
        using var context = GetDbContext();
        context.Proveedores.Add(new Proveedores { Id = 1, Nombre = "Proveedor 1" });
        context.Proveedores.Add(new Proveedores { Id = 2, Nombre = "Proveedor 2" });
        await context.SaveChangesAsync();

        var service = new ProveedorService(context, _mapper);

        var result = await service.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_DevuelveProveedorExistente()
    {
        using var context = GetDbContext();
        context.Proveedores.Add(new Proveedores { Id = 1, Nombre = "Proveedor 1" });
        await context.SaveChangesAsync();

        var service = new ProveedorService(context, _mapper);

        var result = await service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Proveedor 1", result.Nombre);
    }

    [Fact]
    public async Task CreateAsync_CreaProveedorCorrectamente()
    {
        using var context = GetDbContext();
        var service = new ProveedorService(context, _mapper);

        var nuevoProveedor = new ProveedorDto { Nombre = "Nuevo Proveedor" };

        var result = await service.CreateAsync(nuevoProveedor);

        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("Nuevo Proveedor", result.Nombre);
    }

    [Fact]
    public async Task UpdateAsync_ActualizaProveedorExistente()
    {
        using var context = GetDbContext();
        context.Proveedores.Add(new Proveedores { Id = 1, Nombre = "Proveedor 1" });
        await context.SaveChangesAsync();

        var service = new ProveedorService(context, _mapper);
        var updateDto = new ProveedorDto { Nombre = "Proveedor Actualizado" };

        var result = await service.UpdateAsync(1, updateDto);

        Assert.True(result);
        var proveedor = await context.Proveedores.FindAsync(1);
        Assert.Equal("Proveedor Actualizado", proveedor.Nombre);
    }

    [Fact]
    public async Task DeleteAsync_EliminaProveedorExistente()
    {
        using var context = GetDbContext();
        context.Proveedores.Add(new Proveedores { Id = 1, Nombre = "Proveedor 1" });
        await context.SaveChangesAsync();

        var service = new ProveedorService(context, _mapper);

        var result = await service.DeleteAsync(1);

        Assert.True(result);
        var proveedor = await context.Proveedores.FindAsync(1);
        Assert.Null(proveedor);
    }
}
