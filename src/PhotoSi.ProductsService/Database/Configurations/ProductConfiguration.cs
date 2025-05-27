using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoSi.ProductsService.Models;

namespace PhotoSi.ProductsService.Database.Configurations
{
    internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Code);
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryCode)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
