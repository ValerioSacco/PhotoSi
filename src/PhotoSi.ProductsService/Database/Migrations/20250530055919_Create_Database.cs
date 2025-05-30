using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PhotoSi.ProductsService.Database.Migrations
{
    /// <inheritdoc />
    public partial class Create_Database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Version = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    CategoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Version = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "Version" },
                values: new object[,]
                {
                    { new Guid("1c3e24c8-b0c5-4c9b-8b28-f5c5ec2c819a"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Biglietti auguri per ricorrenze", "Biglietti", 1 },
                    { new Guid("59cf0ad7-89a8-4dd5-83da-9cb50608080b"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Gadget per il tempo libero", "Gadgets", 1 },
                    { new Guid("b3eaf4bd-2e57-4041-8cf0-6a19a55c9fb9"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Stampa semplice di una foto", "Stampe", 1 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "ImageUrl", "Name", "Price", "Version" },
                values: new object[,]
                {
                    { new Guid("00f04905-a718-4d28-b652-3237cd4ccbee"), new Guid("1c3e24c8-b0c5-4c9b-8b28-f5c5ec2c819a"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Biglietto di auguri personalizzato per ogni occasione", "https://example.com/images/biglietto-auguri.jpg", "Biglietto di Auguri", 5.00m, 1 },
                    { new Guid("3e1a498f-9008-4030-bea9-9f79e5e92eb3"), new Guid("b3eaf4bd-2e57-4041-8cf0-6a19a55c9fb9"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Stampa di alta qualità su tela per un effetto artistico", "https://example.com/images/stampa-tela.jpg", "Stampa su Tela", 45.00m, 1 },
                    { new Guid("4a14eaac-3f71-41dc-a5ac-c0f8e6c2fefd"), new Guid("1c3e24c8-b0c5-4c9b-8b28-f5c5ec2c819a"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Album fotografico personalizzato per conservare i ricordi", "https://example.com/images/album-fotografico.jpg", "Album Fotografico", 25.00m, 1 },
                    { new Guid("56abd4b5-1c48-4296-84cf-ee05e7862780"), new Guid("59cf0ad7-89a8-4dd5-83da-9cb50608080b"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Calendario personalizzato da tavolo", "https://example.com/images/calendario-tavolo.jpg", "Calendario da Tavolo", 20.00m, 1 },
                    { new Guid("7da4e09c-70a4-49e9-8079-e2c303dc13a5"), new Guid("59cf0ad7-89a8-4dd5-83da-9cb50608080b"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tazza con stampa personalizzata", "https://example.com/images/tazza-personalizzata.jpg", "Tazza Personalizzata", 15.00m, 1 },
                    { new Guid("ccbd7f1e-be07-4e8e-ab99-9ddc97184ebd"), new Guid("b3eaf4bd-2e57-4041-8cf0-6a19a55c9fb9"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Stampa di una foto in formato 10x15 cm", "https://example.com/images/stampa-foto-10x15.jpg", "Stampa Foto 10x15", 10.50m, 1 },
                    { new Guid("fa3ffd88-f6a5-4599-9408-e6d655588b52"), new Guid("b3eaf4bd-2e57-4041-8cf0-6a19a55c9fb9"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Poster di grandi dimensioni per decorare le pareti", "https://example.com/images/poster-50x70.jpg", "Poster 50x70", 30.00m, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
