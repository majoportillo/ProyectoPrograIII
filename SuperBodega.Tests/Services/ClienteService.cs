using Xunit;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using SuperBodega.Admin.API.Data;
using SuperBodega.Admin.API.Services;
using SuperBodega.Admin.API.Dtos;
using SuperBodega.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SuperBodega.Admin.API.Tests.Services
{
    public class ClienteServiceTests
    {
        private readonly IMapper _mapper;

        public ClienteServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Cliente, ClienteDto>().ReverseMap();
            });
            _mapper = config.CreateMapper();
        }

        private SuperBodegaDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<SuperBodegaDbContext>()
                .UseInMemoryDatabase(databaseName: "ClienteDbTest")
                .Options;
            return new SuperBodegaDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_DevuelveClientes()
        {
            using var context = GetDbContext();
            context.Clientes.Add(new Cliente { Id = 1, Nombre = "Cliente 1" });
            context.Clientes.Add(new Cliente { Id = 2, Nombre = "Cliente 2" });
            await context.SaveChangesAsync();

            var service = new ClienteService(context, _mapper);

            var result = await service.GetAllAsync();

            Assert.Equal(2, result.Count());
        }

        // ... resto de tus tests
    }
}

