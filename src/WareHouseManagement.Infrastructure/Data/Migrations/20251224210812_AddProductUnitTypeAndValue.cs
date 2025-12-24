﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WareHouseManagement.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductUnitTypeAndValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Volume",
                table: "Products");

            migrationBuilder.AddColumn<decimal>(
                name: "UnitValue",
                table: "Products",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitType",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UnitValue",
                table: "Products");

            migrationBuilder.AddColumn<decimal>(
                name: "Volume",
                table: "Products",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
