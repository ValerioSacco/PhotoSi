using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoSi.ProductsService.Models;

namespace PhotoSi.ProductsService.Database.Configurations
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(c => c.Id);

            builder.HasIndex(c => c.Name)
                .IsUnique();
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(200);

            //builder.Property(p => p.Version)
            //    .IsConcurrencyToken();

            builder.HasMany(c => c.Products);


            builder.HasData
            (
                new Category
                {
                    Id = Guid.Parse("b3eaf4bd-2e57-4041-8cf0-6a19a55c9fb9"),
                    Name = "Stampe",
                    Description = "Stampa semplice di una foto",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Category
                {
                    Id = Guid.Parse("59cf0ad7-89a8-4dd5-83da-9cb50608080b"),
                    Name = "Gadgets",
                    Description = "Gadget per il tempo libero",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Category
                {
                    Id = Guid.Parse("1c3e24c8-b0c5-4c9b-8b28-f5c5ec2c819a"),
                    Name = "Biglietti",
                    Description = "Biglietti auguri per ricorrenze",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
