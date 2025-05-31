using Xunit;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task GetAllAsync_DeberiaRetornarVentas()
        {
            // Crear opciones de contexto con base de datos única por test
            var options = new DbContextOptionsBuilder<SuperBodegaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Usar bloque using para contexto en memoria
            using (var context = new SuperBodegaDbContext(options))
            {
                // Agregar producto relacionado
                var producto = new Producto { Id = 1, Nombre = "Producto Test", Stock = 10 };
                context.Productos.Add(producto);

                // Agregar venta con detalle vinculado al producto
                var venta = new Venta
                {
                    Id = 1,
                    Estado = "Recibido",
                    Fecha = DateTime.Now,
                    ClienteId = 1,
                    Total = 100,
                    DetalleVenta = new List<DetalleVenta>
                    {
                        new DetalleVenta
                        {
                            ProductoId = 1,
                            Cantidad = 2,
                            PrecioUnitario = 50
                        }
                    }
                };
                context.Ventas.Add(venta);

                await context.SaveChangesAsync();
            }

            // Reabrir contexto para el test real
            using (var context = new SuperBodegaDbContext(options))
            {
                var service = new VentaService(context, _mapper);

                // Act
                var ventas = await service.GetAllAsync();

                // Assert
                Assert.NotNull(ventas);
                var lista = ventas.ToList();
                Assert.Single(lista);

                var venta = lista.First();
                Assert.Equal(1, venta.Detalle.First().ProductoId);
                Assert.Equal("Producto Test", venta.Detalle.First().NombreProducto);
            }
        }
    }
}
