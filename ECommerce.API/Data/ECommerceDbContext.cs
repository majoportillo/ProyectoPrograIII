using Microsoft.EntityFrameworkCore;
using SuperBodega.Models;
using SuperBodega.Models.Dtos;

namespace ECommerce.API.Data
{
    public class ECommerceDbContext : DbContext
    {
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options) { }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetalleVenta { get; set; }
        public DbSet<DetalleCompra> DetalleCompra { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Carrito>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Cliente)
                      .WithMany(c => c.Carritos)
                      .HasForeignKey(e => e.ClienteId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Producto)
                      .WithMany(p => p.Carritos)
                      .HasForeignKey(e => e.ProductoId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<DetalleVenta>(entity =>
            {
                entity.HasKey(e => new { e.VentaId, e.ProductoId });

                entity.HasOne(e => e.Venta)
                      .WithMany(v => v.DetalleVenta)
                      .HasForeignKey(e => e.VentaId);

                entity.HasOne(e => e.Producto)
                      .WithMany(p => p.DetalleVenta)
                      .HasForeignKey(e => e.ProductoId);
            });
            modelBuilder.Entity<DetalleCompra>(entity =>
            {
                entity.HasKey(e => new { e.CompraId, e.ProductoId });

                entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10,2)");

                entity.HasOne(e => e.Compra)
                      .WithMany(c => c.DetalleCompras)
                      .HasForeignKey(e => e.CompraId);

                entity.HasOne(e => e.Producto)
                      .WithMany(p => p.DetalleCompras)
                      .HasForeignKey(e => e.ProductoId);
            });

        }
    }
}

