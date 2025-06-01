using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoSi.OrdersService.Models;

namespace PhotoSi.ProductsService.Database.Configurations
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Currency)
                .IsRequired()
                .HasMaxLength(3);

            //builder.Property(p => p.Version)
            //    .IsConcurrencyToken();

            builder.HasMany(o => o.OrderLines)
                .WithOne(ol => ol.Order)
                .HasForeignKey(ol => ol.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.User);

            builder.HasData(
                new Order
                {
                    Id = Guid.Parse("186aa5d6-77dd-4b90-bc69-b487ba9c3893"),
                    Currency = "EUR",
                    UserId = Guid.Parse("0d4bdc20-95dd-4fe3-98b3-ffac3eadae6d"),
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
