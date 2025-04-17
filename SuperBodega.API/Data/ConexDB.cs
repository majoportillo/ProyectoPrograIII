using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SuperBodega.API.Models;

namespace SuperBodega.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Producto> Productos { get; set; }
        //public DbSet<Proveedor> Proveedores { get; set; } 
    }
}


