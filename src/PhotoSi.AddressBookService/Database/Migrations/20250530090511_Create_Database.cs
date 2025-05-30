using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PhotoSi.AddressBookService.Database.Migrations
{
    /// <inheritdoc />
    public partial class Create_Database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
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
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "City", "Country", "CreatedAt", "PostalCode", "Street", "Version" },
                values: new object[,]
                {
                    { new Guid("4e0b628f-89f6-4b30-a2a3-c5ee53af9882"), "Torino", "Italia", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "40100", "Via Torino 5", 1 },
                    { new Guid("ad7ff260-682e-407e-86e5-e03891f100a4"), "Napoli", "Italia", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "80100", "Via Napoli 3", 1 },
                    { new Guid("e1d0862b-d5f8-426a-af8d-a05f03d3ea65"), "Milano", "Italia", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "20100", "Via Milano 2", 1 },
                    { new Guid("e72b435d-1adb-49e6-811a-77e53c211ff2"), "Bologna", "Italia", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "50100", "Via Bologna 4", 1 },
                    { new Guid("e9ad62c4-118a-4cb6-9493-5deeecfcd791"), "Torino", "Italia", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "90100", "Via Genova 6", 1 },
                    { new Guid("f2b51297-7948-4816-98da-e8502aba672e"), "Roma", "Italia", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "00100", "Via Roma 1", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_PostalCode_Street",
                table: "Addresses",
                columns: new[] { "PostalCode", "Street" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}
