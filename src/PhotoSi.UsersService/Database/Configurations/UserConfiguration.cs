using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoSi.UsersService.Models;

namespace PhotoSi.UsersService.Database.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.ProfilePictureUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(u => u.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired(false);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(u => u.ShipmentAddress)
                .WithOne(sa => sa.User)
                .HasForeignKey<User>(u => u.ShipmentAddressId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasData(
                new User
                {
                    Id = Guid.Parse("0d4bdc20-95dd-4fe3-98b3-ffac3eadae6d"),
                    Username = "User01",
                    FirstName = "Mario",
                    LastName = "Rossi",
                    ProfilePictureUrl = "https://example.com/images/user01.jpg",
                    PhoneNumber = "+391234567890",
                    ShipmentAddressId = Guid.Parse("f2b51297-7948-4816-98da-e8502aba672e"),
                    CreatedAt = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc)
                },
                new User
                {
                    Id = Guid.Parse("dc1dc650-ee84-4f3d-9cca-a0baf9421d4e"),
                    Username = "User02",
                    FirstName = "Luca",
                    LastName = "Bianchi",
                    ProfilePictureUrl = "https://example.com/images/user02.jpg",
                    PhoneNumber = "+391234567891",
                    ShipmentAddressId = Guid.Parse("e1d0862b-d5f8-426a-af8d-a05f03d3ea65"),
                    CreatedAt = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc)
                },
                new User
                {
                    Id = Guid.Parse("57b9385d-6b77-4db8-a1a0-510d54631257"),
                    Username = "User03",
                    FirstName = "Giulia",
                    LastName = "Verdi",
                    ProfilePictureUrl = "https://example.com/images/user03.jpg",
                    PhoneNumber = "+391234567892",
                    ShipmentAddressId = Guid.Parse("ad7ff260-682e-407e-86e5-e03891f100a4"),
                    CreatedAt = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
