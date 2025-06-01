using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoSi.ProductsService.Models;

namespace PhotoSi.ProductsService.Database.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasData(
                new Product
                {
                    Id = Guid.Parse("ccbd7f1e-be07-4e8e-ab99-9ddc97184ebd"),
                    Name = "Stampa Foto 10x15",
                    Description = "Stampa di una foto in formato 10x15 cm",
                    Price = 10.50m,
                    ImageUrl = "https://example.com/images/stampa-foto-10x15.jpg",
                    CategoryId = Guid.Parse("b3eaf4bd-2e57-4041-8cf0-6a19a55c9fb9"),
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = Guid.Parse("7da4e09c-70a4-49e9-8079-e2c303dc13a5"),
                    Name = "Tazza Personalizzata",
                    Description = "Tazza con stampa personalizzata",
                    Price = 15.00m,
                    ImageUrl = "https://example.com/images/tazza-personalizzata.jpg",
                    CategoryId = Guid.Parse("59cf0ad7-89a8-4dd5-83da-9cb50608080b"),
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = Guid.Parse("00f04905-a718-4d28-b652-3237cd4ccbee"),
                    Name = "Biglietto di Auguri",
                    Description = "Biglietto di auguri personalizzato per ogni occasione",
                    Price = 5.00m,
                    ImageUrl = "https://example.com/images/biglietto-auguri.jpg",
                    CategoryId = Guid.Parse("1c3e24c8-b0c5-4c9b-8b28-f5c5ec2c819a"),
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = Guid.Parse("56abd4b5-1c48-4296-84cf-ee05e7862780"),
                    Name = "Calendario da Tavolo",
                    Description = "Calendario personalizzato da tavolo",
                    Price = 20.00m,
                    ImageUrl = "https://example.com/images/calendario-tavolo.jpg",
                    CategoryId = Guid.Parse("59cf0ad7-89a8-4dd5-83da-9cb50608080b"),
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = Guid.Parse("fa3ffd88-f6a5-4599-9408-e6d655588b52"),
                    Name = "Poster 50x70",
                    Description = "Poster di grandi dimensioni per decorare le pareti",
                    Price = 30.00m,
                    ImageUrl = "https://example.com/images/poster-50x70.jpg",
                    CategoryId = Guid.Parse("b3eaf4bd-2e57-4041-8cf0-6a19a55c9fb9"),
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = Guid.Parse("4a14eaac-3f71-41dc-a5ac-c0f8e6c2fefd"),
                    Name = "Album Fotografico",
                    Description = "Album fotografico personalizzato per conservare i ricordi",
                    Price = 25.00m,
                    ImageUrl = "https://example.com/images/album-fotografico.jpg",
                    CategoryId = Guid.Parse("1c3e24c8-b0c5-4c9b-8b28-f5c5ec2c819a"),
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = Guid.Parse("3e1a498f-9008-4030-bea9-9f79e5e92eb3"),
                    Name = "Stampa su Tela",
                    Description = "Stampa di alta qualità su tela per un effetto artistico",
                    Price = 45.00m,
                    ImageUrl = "https://example.com/images/stampa-tela.jpg",
                    CategoryId = Guid.Parse("b3eaf4bd-2e57-4041-8cf0-6a19a55c9fb9"),
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
