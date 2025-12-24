using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WareHouseManagement.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUnitTypeToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UnitType",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitType",
                table: "Products");
        }
    }
}
