using Microsoft.EntityFrameworkCore;
using SuperBodega.Admin.API.Models;
using SuperBodega.Models;

namespace SuperBodega.Admin.API.Data
{
    public class SuperBodegaDbContext : DbContext
    {
        public SuperBodegaDbContext(DbContextOptions<SuperBodegaDbContext> options)
            : base(options) { }

        // 🔗 Entidades principales

        public virtual DbSet<User> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Proveedores> Proveedores { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<DetalleCompra> DetalleCompras { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetalleVenta { get; set; }
        public DbSet<Carrito> Carritos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UsuarioNombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsRequired();
            });

            // 🔹 Carrito
            modelBuilder.Entity<Carrito>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FechaAgregado)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(e => e.Cliente)
                      .WithMany(c => c.Carritos)
                      .HasForeignKey(e => e.ClienteId);

                entity.HasOne(e => e.Producto)
                      .WithMany(p => p.Carritos)
                      .HasForeignKey(e => e.ProductoId);
            });

            // 🔹 Cliente
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.Email).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.Direccion).HasMaxLength(200).IsUnicode(false);
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // 🔹 Compra
            modelBuilder.Entity<Compra>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
                entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

                entity.HasOne(e => e.Proveedor)
                      .WithMany(p => p.Compras)
                      .HasForeignKey(e => e.ProveedorId);
            });

            // 🔹 DetalleCompra
            modelBuilder.Entity<DetalleCompra>(entity =>
            {
                entity.HasKey(e => new { e.CompraId, e.ProductoId });
                entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");

                entity.HasOne(e => e.Compra)
                      .WithMany(c => c.DetalleCompras)
                      .HasForeignKey(e => e.CompraId);

                entity.HasOne(e => e.Producto)
                      .WithMany(p => p.DetalleCompras)
                      .HasForeignKey(e => e.ProductoId);
            });

            // 🔹 DetalleVenta
            modelBuilder.Entity<DetalleVenta>(entity =>
            {
                entity.HasKey(e => new { e.VentaId, e.ProductoId });
                entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");

                entity.HasOne(e => e.Venta)
                      .WithMany(v => v.DetalleVenta)
                      .HasForeignKey(e => e.VentaId);

                entity.HasOne(e => e.Producto)
                      .WithMany(p => p.DetalleVenta)
                      .HasForeignKey(e => e.ProductoId);
            });

            // 🔹 Producto
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.Categoria).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Descripcion).HasColumnType("text");
                entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.Stock).HasDefaultValue(0);

                entity.HasOne(e => e.Proveedor)
                      .WithMany(p => p.Productos)
                      .HasForeignKey(e => e.ProveedorId);
            });

            // 🔹 Proveedores
            modelBuilder.Entity<Proveedores>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.Direccion).HasMaxLength(200).IsUnicode(false);
                entity.Property(e => e.Telefono).HasMaxLength(100).IsUnicode(false);
            });

            // 🔹 Venta
            modelBuilder.Entity<Venta>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
                entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.Estado)
                      .HasMaxLength(20)
                      .IsUnicode(false)
                      .HasDefaultValue("Recibido");

                entity.HasOne(e => e.Cliente)
                      .WithMany(c => c.Venta)
                      .HasForeignKey(e => e.ClienteId);
            });
        }
    }
}

