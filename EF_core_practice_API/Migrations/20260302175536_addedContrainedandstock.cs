using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EF_core_practice_API.Migrations
{
    /// <inheritdoc />
    public partial class addedContrainedandstock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddCheckConstraint(
                name: "ck_products_stock_NonNegative",
                table: "Products",
                sql: "[Stock]>=0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_products_stock_NonNegative",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Products");
        }
    }
}
