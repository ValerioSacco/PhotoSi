using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoSi.AddressBookService.Models;

namespace PhotoSi.AddressBookService.Database.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Street)
                .IsRequired();

            builder.Property(a => a.City)
                .IsRequired();

            builder.Property(a => a.PostalCode)
                .IsRequired();

            builder.Property(a => a.Country)
                .IsRequired();

            builder.HasIndex(a => new { a.PostalCode, a.Street })
                .IsUnique();

            builder.HasData(
                new Address
                {
                    Id = Guid.Parse("f2b51297-7948-4816-98da-e8502aba672e"),
                    Street = "Via Roma 1",
                    City = "Roma",
                    PostalCode = "00100",
                    Country = "Italia",
                    CreatedAt = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc)
                },
                new Address
                {
                    Id = Guid.Parse("e1d0862b-d5f8-426a-af8d-a05f03d3ea65"),
                    Street = "Via Milano 2",
                    City = "Milano",
                    PostalCode = "20100",
                    Country = "Italia",
                    CreatedAt = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc)
                },
                new Address
                {
                    Id = Guid.Parse("ad7ff260-682e-407e-86e5-e03891f100a4"),
                    Street = "Via Napoli 3",
                    City = "Napoli",
                    PostalCode = "80100",
                    Country = "Italia",
                    CreatedAt = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc)
                },
                new Address
                {
                    Id = Guid.Parse("e72b435d-1adb-49e6-811a-77e53c211ff2"),
                    Street = "Via Bologna 4",
                    City = "Bologna",
                    PostalCode = "50100",
                    Country = "Italia",
                    CreatedAt = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc)
                },
                new Address
                {
                    Id = Guid.Parse("4e0b628f-89f6-4b30-a2a3-c5ee53af9882"),
                    Street = "Via Torino 5",
                    City = "Torino",
                    PostalCode = "40100",
                    Country = "Italia",
                    CreatedAt = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc)
                },
                new Address
                {
                    Id = Guid.Parse("e9ad62c4-118a-4cb6-9493-5deeecfcd791"),
                    Street = "Via Genova 6",
                    City = "Torino",
                    PostalCode = "90100",
                    Country = "Italia",
                    CreatedAt = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
