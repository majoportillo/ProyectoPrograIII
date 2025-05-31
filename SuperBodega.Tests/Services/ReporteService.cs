using Xunit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SuperBodega.Admin.API.Data;
using SuperBodega.Admin.API.Models;
using SuperBodega.Admin.API.Dtos;
using SuperBodega.Admin.API.Services;
using SuperBodega.Models;

namespace SuperBodega.Tests.Services
{
    public class ReporteServiceTests
    {
        [Fact]
        public async Task ObtenerPorClienteAsync_DeberiaRetornarVentasDelCliente()
        {
            // Configurar DbContext en memoria con datos de prueba
            var options = new DbContextOptionsBuilder<SuperBodegaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new SuperBodegaDbContext(options))
            {
                // Crear cliente de prueba
                var cliente = new Cliente { Id = 1, Nombre = "Cliente Test" };
                context.Clientes.Add(cliente);

                // Crear ventas para ese cliente
                context.Ventas.AddRange(
                    new Venta
                    {
                        Id = 1,
                        ClienteId = cliente.Id,
                        Fecha = DateTime.Now,
                        Estado = "Recibido",
                        Total = 100
                    },
                    new Venta
                    {
                        Id = 2,
                        ClienteId = cliente.Id,
                        Fecha = DateTime.Now.AddDays(-1),
                        Estado = "Entregado",
                        Total = 200
                    }
                );

                await context.SaveChangesAsync();
            }

            using (var context = new SuperBodegaDbContext(options))
            {
                var service = new ReporteService(context);

                // Act
                var resultado = await service.ObtenerPorClienteAsync(1);

                // Assert
                Assert.NotNull(resultado);
                var lista = resultado.ToList();
                Assert.Equal(2, lista.Count);

                Assert.All(lista, v => Assert.Equal("Cliente Test", v.Cliente));
                Assert.Contains(lista, v => v.Estado == "Recibido");
                Assert.Contains(lista, v => v.Estado == "Entregado");
            }
        }
    }
}
