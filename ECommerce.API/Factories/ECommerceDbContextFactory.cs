using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ECommerce.API.Data;
using SuperBodega.Models.Dtos;

namespace ECommerce.API.Factories
{
    public class ECommerceDbContextFactory : IDesignTimeDbContextFactory<ECommerceDbContext>
    {
        public ECommerceDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ECommerceDbContext>();

            optionsBuilder.UseSqlServer(
                "Server=localhost,1433;Database=SuperBodegaDB;User Id=sa;Password=Admin123;TrustServerCertificate=True;");

            return new ECommerceDbContext(optionsBuilder.Options);
        }
    }
}


