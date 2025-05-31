using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoSi.OrdersService.Models;

namespace PhotoSi.OrdersService.Database.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.IsAvailable)
                .IsRequired()
                .HasDefaultValue(true);

            builder.OwnsOne(u => u.Address, address =>
            {
                address.Property(a => a.Street)
                    .HasColumnName("Street")
                    .IsRequired();

                address.Property(a => a.City)
                    .HasColumnName("City")
                    .IsRequired();

                address.Property(a => a.Country)
                    .HasColumnName("Country")
                    .IsRequired();

                address.Property(a => a.PostalCode)
                    .HasColumnName("PostalCode")
                    .IsRequired();
            });


            builder.HasData(
                new User
                {
                    Id = Guid.Parse("0d4bdc20-95dd-4fe3-98b3-ffac3eadae6d"),
                    FirstName = "Mario",
                    LastName = "Rossi",
                    IsAvailable = true,
                    CreatedAt = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc)
                },
                new User
                {
                    Id = Guid.Parse("dc1dc650-ee84-4f3d-9cca-a0baf9421d4e"),
                    FirstName = "Luca",
                    LastName = "Bianchi",
                    IsAvailable = true,
                    CreatedAt = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc)
                },
                new User
                {
                    Id = Guid.Parse("57b9385d-6b77-4db8-a1a0-510d54631257"),
                    FirstName = "Giulia",
                    LastName = "Verdi",
                    IsAvailable = true,
                    CreatedAt = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc)
                }
            );


            builder.OwnsOne(u => u.Address).HasData(
                new ShipmentAddress
                {
                    UserId = Guid.Parse("0d4bdc20-95dd-4fe3-98b3-ffac3eadae6d"),
                    Country = "Italia",
                    City = "Roma",
                    PostalCode = "00100",
                    Street = "Via Roma 1"
                },
                new ShipmentAddress
                {
                    UserId = Guid.Parse("dc1dc650-ee84-4f3d-9cca-a0baf9421d4e"),
                    Country = "Italia",
                    City = "Milano",
                    PostalCode = "20100",
                    Street = "Via Milano 2"
                },
                new ShipmentAddress
                {
                    UserId = Guid.Parse("57b9385d-6b77-4db8-a1a0-510d54631257"),
                    Country = "Italia",
                    City = "Napoli",
                    PostalCode = "80100",
                    Street = "Via Napoli 3"
                }
            );
        }
    }
}
