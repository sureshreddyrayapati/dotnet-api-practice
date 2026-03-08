using Microsoft.EntityFrameworkCore;
namespace EF_core_practice_API.Model.DBContext
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Stock).IsRequired();
                entity.ToTable(t => t.HasCheckConstraint("ck_products_stock_NonNegative", "[Stock]>=0"));
                
                
            });
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name)
                .IsUnique();
            modelBuilder.Entity<Product>()
                .HasQueryFilter(p => !p.IsDeleted);
        }

    }
}
