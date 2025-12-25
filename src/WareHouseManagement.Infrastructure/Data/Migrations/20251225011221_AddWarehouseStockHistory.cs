using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WareHouseManagement.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWarehouseStockHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantityInBottles",
                table: "WarehouseStocks");

            migrationBuilder.DropColumn(
                name: "QuantityInBoxes",
                table: "WarehouseStocks");

            migrationBuilder.DropColumn(
                name: "AlcoholPercentage",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BottlesPerBox",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UnitType",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UnitValue",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "QuantityInBottles",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "QuantityInBoxes",
                table: "OrderItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "ManufacturerId",
                table: "WarehouseStocks",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "WarehouseStocks",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Warehouses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "UnitTypeRuleId",
                table: "Products",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "OrderItems",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ContactInfo",
                table: "Manufacturers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Manufacturers",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "CompanyLocations",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "ContactPerson",
                table: "CompanyLocations",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AlcoholicStockDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseStockId = table.Column<Guid>(type: "uuid", nullable: false),
                    BatchNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ExciseStampNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CertificateNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    StorageTemperature = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlcoholicStockDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlcoholicStockDetails_WarehouseStocks_WarehouseStockId",
                        column: x => x.WarehouseStockId,
                        principalTable: "WarehouseStocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PackagingDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseStockId = table.Column<Guid>(type: "uuid", nullable: false),
                    PackagingType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UnitsPerPackage = table.Column<int>(type: "integer", nullable: false),
                    FullPackagesCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    PartialPackagesCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    UnitsInPartialPackages = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackagingDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackagingDetails_WarehouseStocks_WarehouseStockId",
                        column: x => x.WarehouseStockId,
                        principalTable: "WarehouseStocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    CountryOfOrigin = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ProductType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ShelfLifeMonths = table.Column<int>(type: "integer", nullable: true),
                    AdditionalNotes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnitTypeRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UnitType = table.Column<int>(type: "integer", nullable: false),
                    NameKa = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Abbreviation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    AllowOnlyWholeNumbers = table.Column<bool>(type: "boolean", nullable: false),
                    MinValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MaxValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    DefaultValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitTypeRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseStockHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseStockId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionType = table.Column<int>(type: "integer", nullable: false),
                    QuantityChange = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    QuantityBefore = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    QuantityAfter = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: true),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    PerformedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseStockHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseStockHistories_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_WarehouseStockHistories_WarehouseStocks_WarehouseStockId",
                        column: x => x.WarehouseStockId,
                        principalTable: "WarehouseStocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AlcoholicProductDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductDetailsId = table.Column<Guid>(type: "uuid", nullable: false),
                    AlcoholPercentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    Region = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ServingTemperature = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    QualityClass = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlcoholicProductDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlcoholicProductDetails_ProductDetails_ProductDetailsId",
                        column: x => x.ProductDetailsId,
                        principalTable: "ProductDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "UnitTypeRules",
                columns: new[] { "Id", "Abbreviation", "AllowOnlyWholeNumbers", "CreatedAt", "DefaultValue", "Description", "IsActive", "IsDeleted", "MaxValue", "MinValue", "NameEn", "NameKa", "UnitType", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "ც", true, new DateTime(2025, 12, 25, 1, 12, 21, 216, DateTimeKind.Utc).AddTicks(381), 1m, "პროდუქტის რაოდენობა ცალების მიხედვით", true, false, null, 1m, "Piece", "ცალი", 0, new DateTime(2025, 12, 25, 1, 12, 21, 216, DateTimeKind.Utc).AddTicks(381) },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "ლ", false, new DateTime(2025, 12, 25, 1, 12, 21, 216, DateTimeKind.Utc).AddTicks(381), 0.5m, "პროდუქტის მოცულობა ლიტრებში", true, false, 1000m, 0.001m, "Liter", "ლიტრი", 1, new DateTime(2025, 12, 25, 1, 12, 21, 216, DateTimeKind.Utc).AddTicks(381) },
                    { new Guid("10000000-0000-0000-0000-000000000003"), "მლ", false, new DateTime(2025, 12, 25, 1, 12, 21, 216, DateTimeKind.Utc).AddTicks(381), 500m, "პროდუქტის მოცულობა მილილიტრებში", true, false, 10000m, 1m, "Milliliter", "მილილიტრი", 2, new DateTime(2025, 12, 25, 1, 12, 21, 216, DateTimeKind.Utc).AddTicks(381) },
                    { new Guid("10000000-0000-0000-0000-000000000004"), "კგ", false, new DateTime(2025, 12, 25, 1, 12, 21, 216, DateTimeKind.Utc).AddTicks(381), 1m, "პროდუქტის წონა კილოგრამებში", true, false, 1000m, 0.001m, "Kilogram", "კილოგრამი", 3, new DateTime(2025, 12, 25, 1, 12, 21, 216, DateTimeKind.Utc).AddTicks(381) },
                    { new Guid("10000000-0000-0000-0000-000000000005"), "გ", false, new DateTime(2025, 12, 25, 1, 12, 21, 216, DateTimeKind.Utc).AddTicks(381), 100m, "პროდუქტის წონა გრამებში", true, false, 100000m, 1m, "Gram", "გრამი", 4, new DateTime(2025, 12, 25, 1, 12, 21, 216, DateTimeKind.Utc).AddTicks(381) },
                    { new Guid("10000000-0000-0000-0000-000000000006"), "ყთ", true, new DateTime(2025, 12, 25, 1, 12, 21, 216, DateTimeKind.Utc).AddTicks(381), 1m, "პროდუქტის რაოდენობა ყუთების მიხედვით", true, false, null, 1m, "Box", "ყუთი", 5, new DateTime(2025, 12, 25, 1, 12, 21, 216, DateTimeKind.Utc).AddTicks(381) },
                    { new Guid("10000000-0000-0000-0000-000000000007"), "პქ", true, new DateTime(2025, 12, 25, 1, 12, 21, 216, DateTimeKind.Utc).AddTicks(381), 1m, "პროდუქტის რაოდენობა პაკეტების მიხედვით", true, false, null, 1m, "Package", "პაკეტი", 6, new DateTime(2025, 12, 25, 1, 12, 21, 216, DateTimeKind.Utc).AddTicks(381) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_UnitTypeRuleId",
                table: "Products",
                column: "UnitTypeRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_AlcoholicProductDetails_ProductDetailsId",
                table: "AlcoholicProductDetails",
                column: "ProductDetailsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AlcoholicStockDetails_WarehouseStockId",
                table: "AlcoholicStockDetails",
                column: "WarehouseStockId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PackagingDetails_WarehouseStockId",
                table: "PackagingDetails",
                column: "WarehouseStockId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_ProductId",
                table: "ProductDetails",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnitTypeRules_UnitType",
                table: "UnitTypeRules",
                column: "UnitType",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseStockHistories_OrderId",
                table: "WarehouseStockHistories",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseStockHistories_TransactionDate",
                table: "WarehouseStockHistories",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseStockHistories_TransactionType",
                table: "WarehouseStockHistories",
                column: "TransactionType");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseStockHistories_WarehouseStockId",
                table: "WarehouseStockHistories",
                column: "WarehouseStockId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_UnitTypeRules_UnitTypeRuleId",
                table: "Products",
                column: "UnitTypeRuleId",
                principalTable: "UnitTypeRules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_UnitTypeRules_UnitTypeRuleId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "AlcoholicProductDetails");

            migrationBuilder.DropTable(
                name: "AlcoholicStockDetails");

            migrationBuilder.DropTable(
                name: "PackagingDetails");

            migrationBuilder.DropTable(
                name: "UnitTypeRules");

            migrationBuilder.DropTable(
                name: "WarehouseStockHistories");

            migrationBuilder.DropTable(
                name: "ProductDetails");

            migrationBuilder.DropIndex(
                name: "IX_Products_UnitTypeRuleId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "WarehouseStocks");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UnitTypeRuleId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ContactInfo",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "ContactPerson",
                table: "CompanyLocations");

            migrationBuilder.AlterColumn<Guid>(
                name: "ManufacturerId",
                table: "WarehouseStocks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuantityInBottles",
                table: "WarehouseStocks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuantityInBoxes",
                table: "WarehouseStocks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "AlcoholPercentage",
                table: "Products",
                type: "numeric(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "BottlesPerBox",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnitType",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitValue",
                table: "Products",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuantityInBottles",
                table: "OrderItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuantityInBoxes",
                table: "OrderItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "CompanyLocations",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
