using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PhotoSi.OrdersService.Database.Migrations
{
    /// <inheritdoc />
    public partial class Create_Database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CategoryName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    IsAvailable = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Version = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    Street = table.Column<string>(type: "TEXT", nullable: false),
                    PostalCode = table.Column<string>(type: "TEXT", nullable: false),
                    IsAvailable = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Version = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Version = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Version = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderLines_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderLines_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryName", "CreatedAt", "Description", "IsAvailable", "Name", "Price", "Version" },
                values: new object[,]
                {
                    { new Guid("00f04905-a718-4d28-b652-3237cd4ccbee"), "Biglietti", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Biglietto di auguri personalizzato per ogni occasione", true, "Biglietto di Auguri", 5.00m, 1 },
                    { new Guid("3e1a498f-9008-4030-bea9-9f79e5e92eb3"), "Stampe", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Stampa di alta qualità su tela per un effetto artistico", true, "Stampa su Tela", 45.00m, 1 },
                    { new Guid("56abd4b5-1c48-4296-84cf-ee05e7862780"), "Gadgets", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Calendario personalizzato da tavolo", true, "Calendario da Tavolo", 20.00m, 1 },
                    { new Guid("7da4e09c-70a4-49e9-8079-e2c303dc13a5"), "Gadgets", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Tazza con stampa personalizzata", true, "Tazza Personalizzata", 15.00m, 1 },
                    { new Guid("b3eaf4bd-2e57-4041-8cf0-6a19a55c9fb9"), "Biglietti", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Album fotografico personalizzato per conservare i tuoi ricordi", true, "Album Fotografico", 25.00m, 1 },
                    { new Guid("ccbd7f1e-be07-4e8e-ab99-9ddc97184ebd"), "Stampe", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Stampa di una foto in formato 10x15 cm", true, "Stampa Foto 10x15", 10.50m, 1 },
                    { new Guid("fa3ffd88-f6a5-4599-9408-e6d655588b52"), "Stampe", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Poster di grandi dimensioni per decorare le pareti", true, "Poster 50x70", 30.00m, 1 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "City", "Country", "PostalCode", "Street", "CreatedAt", "FirstName", "IsAvailable", "LastName", "Version" },
                values: new object[,]
                {
                    { new Guid("0d4bdc20-95dd-4fe3-98b3-ffac3eadae6d"), "Roma", "Italia", "00100", "Via Roma 1", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Mario", true, "Rossi", 1 },
                    { new Guid("57b9385d-6b77-4db8-a1a0-510d54631257"), "Napoli", "Italia", "80100", "Via Napoli 3", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Giulia", true, "Verdi", 1 },
                    { new Guid("dc1dc650-ee84-4f3d-9cca-a0baf9421d4e"), "Milano", "Italia", "20100", "Via Milano 2", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Luca", true, "Bianchi", 1 }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CreatedAt", "Currency", "UserId", "Version" },
                values: new object[] { new Guid("186aa5d6-77dd-4b90-bc69-b487ba9c3893"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "EUR", new Guid("0d4bdc20-95dd-4fe3-98b3-ffac3eadae6d"), 1 });

            migrationBuilder.InsertData(
                table: "OrderLines",
                columns: new[] { "Id", "CreatedAt", "Notes", "OrderId", "ProductId", "Quantity", "Version" },
                values: new object[,]
                {
                    { new Guid("25ae29d4-90f6-41cf-8820-824825f6db1b"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "First order line", new Guid("186aa5d6-77dd-4b90-bc69-b487ba9c3893"), new Guid("ccbd7f1e-be07-4e8e-ab99-9ddc97184ebd"), 2, 1 },
                    { new Guid("6ebb39f3-c2a3-4859-82eb-ad39335c6cb9"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Second order line", new Guid("186aa5d6-77dd-4b90-bc69-b487ba9c3893"), new Guid("7da4e09c-70a4-49e9-8079-e2c303dc13a5"), 5, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderLines_OrderId",
                table: "OrderLines",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLines_ProductId",
                table: "OrderLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderLines");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
