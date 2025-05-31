using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using SuperBodega.Admin.API.Data;
using SuperBodega.Admin.API.Services;
using SuperBodega.Admin.API.Dtos;
using SuperBodega.Models;

public class ProductoServiceTests
{
    private SuperBodegaDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<SuperBodegaDbContext>()
            .UseInMemoryDatabase(databaseName: "ProductoDbTest")
            .Options;
        return new SuperBodegaDbContext(options);
    }

    [Fact]
    public async Task GetAllAsync_DevuelveTodosLosProductos()
    {
        using var context = GetDbContext();
        context.Productos.Add(new Producto { Id = 1, Nombre = "Producto 1", Precio = 10 });
        context.Productos.Add(new Producto { Id = 2, Nombre = "Producto 2", Precio = 20 });
        await context.SaveChangesAsync();

        var service = new ProductoService(context);

        var result = await service.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_DevuelveProductoExistente()
    {
        using var context = GetDbContext();
        context.Productos.Add(new Producto { Id = 1, Nombre = "Producto 1", Precio = 10 });
        await context.SaveChangesAsync();

        var service = new ProductoService(context);

        var result = await service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Producto 1", result.Nombre);
    }

    [Fact]
    public async Task CreateAsync_CreaProductoCorrectamente()
    {
        using var context = GetDbContext();
        var service = new ProductoService(context);

        var nuevoProducto = new ProductoDto
        {
            Nombre = "Nuevo Producto",
            Categoria = "Cat",
            Descripcion = "Desc",
            Precio = 100,
            Stock = 50,
            ProveedorId = 1
        };

        var result = await service.CreateAsync(nuevoProducto);

        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("Nuevo Producto", result.Nombre);
    }

    [Fact]
    public async Task UpdateAsync_ActualizaProductoExistente()
    {
        using var context = GetDbContext();
        context.Productos.Add(new Producto { Id = 1, Nombre = "Producto 1", Precio = 10 });
        await context.SaveChangesAsync();

        var service = new ProductoService(context);

        var updateDto = new ProductoDto
        {
            Nombre = "Producto Actualizado",
            Categoria = "Cat",
            Descripcion = "Desc",
            Precio = 50,
            Stock = 100,
            ProveedorId = 2
        };

        var result = await service.UpdateAsync(1, updateDto);

        Assert.True(result);
        var producto = await context.Productos.FindAsync(1);
        Assert.Equal("Producto Actualizado", producto.Nombre);
        Assert.Equal(50, producto.Precio);
    }

    [Fact]
    public async Task DeleteAsync_EliminaProductoExistente()
    {
        using var context = GetDbContext();
        context.Productos.Add(new Producto { Id = 1, Nombre = "Producto 1" });
        await context.SaveChangesAsync();

        var service = new ProductoService(context);

        var result = await service.DeleteAsync(1);

        Assert.True(result);
        var producto = await context.Productos.FindAsync(1);
        Assert.Null(producto);
    }
}
