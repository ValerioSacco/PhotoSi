using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoSi.UsersService.Models;

namespace PhotoSi.UsersService.Database.Configurations
{
    public class ShipmentAddressConfiguration : IEntityTypeConfiguration<ShipmentAddress>
    {
        public void Configure(EntityTypeBuilder<ShipmentAddress> builder)
        {
            builder.ToTable("ShipmentAddresses");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Street)
                .IsRequired();

            builder.Property(a => a.City)
                .IsRequired();

            builder.Property(a => a.PostalCode)
                .IsRequired();

            builder.Property(a => a.Country)
                .IsRequired();

            builder.HasData(
                new ShipmentAddress
                {
                    Id = Guid.Parse("f2b51297-7948-4816-98da-e8502aba672e"),
                    Street = "Via Roma 1",
                    City = "Roma",
                    PostalCode = "00100",
                    Country = "Italia",
                    CreatedAt = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc)
                },
                new ShipmentAddress
                {
                    Id = Guid.Parse("e1d0862b-d5f8-426a-af8d-a05f03d3ea65"),
                    Street = "Via Milano 2",
                    City = "Milano",
                    PostalCode = "20100",
                    Country = "Italia",
                    CreatedAt = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc)
                },
                new ShipmentAddress
                {
                    Id = Guid.Parse("ad7ff260-682e-407e-86e5-e03891f100a4"),
                    Street = "Via Napoli 3",
                    City = "Napoli",
                    PostalCode = "80100",
                    Country = "Italia",
                    CreatedAt = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
