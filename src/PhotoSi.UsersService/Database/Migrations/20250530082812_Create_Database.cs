using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PhotoSi.UsersService.Database.Migrations
{
    /// <inheritdoc />
    public partial class Create_Database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShipmentAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Street = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    PostalCode = table.Column<string>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Version = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipmentAddresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    ShipmentAddressId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Version = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_ShipmentAddresses_ShipmentAddressId",
                        column: x => x.ShipmentAddressId,
                        principalTable: "ShipmentAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ShipmentAddresses",
                columns: new[] { "Id", "City", "Country", "CreatedAt", "PostalCode", "Street", "Version" },
                values: new object[,]
                {
                    { new Guid("ad7ff260-682e-407e-86e5-e03891f100a4"), "Napoli", "Italia", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "80100", "Via Napoli 3", 1 },
                    { new Guid("e1d0862b-d5f8-426a-af8d-a05f03d3ea65"), "Milano", "Italia", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "20100", "Via Milano 2", 1 },
                    { new Guid("f2b51297-7948-4816-98da-e8502aba672e"), "Roma", "Italia", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "00100", "Via Roma 1", 1 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "LastName", "PhoneNumber", "ProfilePictureUrl", "ShipmentAddressId", "Username", "Version" },
                values: new object[,]
                {
                    { new Guid("0d4bdc20-95dd-4fe3-98b3-ffac3eadae6d"), new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "", "Mario", "Rossi", "+391234567890", "https://example.com/images/user01.jpg", new Guid("f2b51297-7948-4816-98da-e8502aba672e"), "User01", 1 },
                    { new Guid("57b9385d-6b77-4db8-a1a0-510d54631257"), new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "", "Giulia", "Verdi", "+391234567892", "https://example.com/images/user03.jpg", new Guid("ad7ff260-682e-407e-86e5-e03891f100a4"), "User03", 1 },
                    { new Guid("dc1dc650-ee84-4f3d-9cca-a0baf9421d4e"), new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "", "Luca", "Bianchi", "+391234567891", "https://example.com/images/user02.jpg", new Guid("e1d0862b-d5f8-426a-af8d-a05f03d3ea65"), "User02", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ShipmentAddressId",
                table: "Users",
                column: "ShipmentAddressId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ShipmentAddresses");
        }
    }
}
