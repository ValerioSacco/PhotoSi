using Microsoft.EntityFrameworkCore;
using PhotoSi.ProductsService.Models;

namespace PhotoSi.ProductsService.Database
{
    public sealed class ProductsDbContext : DbContext
    {
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductsDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

    }
}
