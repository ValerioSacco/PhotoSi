using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoSi.ProductsService.Models;

namespace PhotoSi.ProductsService.Database.Configurations
{
    internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Code);
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasMany(c => c.Products);
        }
    }
}
