using Xunit;
using Moq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using SuperBodega.Admin.API.Data;
using SuperBodega.Admin.API.Models;
using SuperBodega.Admin.API.Dtos;
using SuperBodega.Admin.API.Services;
using SuperBodega.Models;

namespace SuperBodega.Tests.Services
{
    public class VentaServiceTests
    {
        private readonly IMapper _mapper;

        public VentaServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Venta, VentaDto>();
                cfg.CreateMap<VentaCreateDto, Venta>();
                // Agrega más mapeos si usas AutoMapper para otras clases
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task GetAllAsync_DeberiaRetornarVentas()
        {
            // Arrange - usar base de datos en memoria
            var options = new DbContextOptionsBuilder<SuperBodegaDbContext>()
                .UseInMemoryDatabase(databaseName: "VentaDb")
                .Options;

            using var context = new SuperBodegaDbContext(options);

            // Agregar datos simulados
            context.Productos.Add(new Producto { Id = 1, Nombre = "Producto Test", Stock = 10 });
            context.Ventas.Add(new Venta
            {
                Id = 1,
                Estado = "Recibido",
                Fecha = System.DateTime.Now,
                ClienteId = 1,
                Total = 100,
                DetalleVenta = new List<DetalleVenta>
                {
                    new DetalleVenta
                    {
                        ProductoId = 1,
                        Cantidad = 2,
                        PrecioUnitario = 50,
                        Producto = new Producto { Id = 1, Nombre = "Producto Test" }
                    }
                }
            });
            await context.SaveChangesAsync();

            var service = new VentaService(context, _mapper);

            // Act
            var ventas = await service.GetAllAsync();

            // Assert
            Assert.NotNull(ventas);
            Assert.Single(ventas);
            Assert.Equal(1, ventas.First().Detalle.First().ProductoId);
        }
    }
}
