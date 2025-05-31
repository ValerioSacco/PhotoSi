using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoSi.OrdersService.Models;

namespace PhotoSi.OrdersService.Database.Configurations
{
    public class OrderLineConfiguration : IEntityTypeConfiguration<OrderLine>
    {
        public void Configure(EntityTypeBuilder<OrderLine> builder)
        {
            builder.ToTable("OrderLines");

            builder.HasKey(ol => ol.Id);

            builder.Property(ol => ol.Notes)
                .HasMaxLength(500);

            builder.Property(ol => ol.Quantity)
                .IsRequired();

            builder.HasOne(ol => ol.Product);

            builder.HasData(
                new OrderLine
                {
                    Id = Guid.Parse("25ae29d4-90f6-41cf-8820-824825f6db1b"),
                    Notes = "First order line",
                    Quantity = 2,
                    OrderId = Guid.Parse("186aa5d6-77dd-4b90-bc69-b487ba9c3893"),
                    ProductId = Guid.Parse("ccbd7f1e-be07-4e8e-ab99-9ddc97184ebd"),
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new OrderLine
                {
                    Id = Guid.Parse("6ebb39f3-c2a3-4859-82eb-ad39335c6cb9"),
                    Notes = "Second order line",
                    Quantity = 5,
                    OrderId = Guid.Parse("186aa5d6-77dd-4b90-bc69-b487ba9c3893"),
                    ProductId = Guid.Parse("7da4e09c-70a4-49e9-8079-e2c303dc13a5"),
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
