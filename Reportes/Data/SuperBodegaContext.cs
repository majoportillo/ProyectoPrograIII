public class SuperBodegaContext : DbContext
{
    public SuperBodegaContext(DbContextOptions<SuperBodegaContext> options) : base(options) {}

    public DbSet<Venta> Ventas { get; set; }
}