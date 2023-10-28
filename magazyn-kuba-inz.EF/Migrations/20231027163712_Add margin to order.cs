using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.EF.Migrations
{
    /// <inheritdoc />
    public partial class Addmargintoorder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "Order",
                newName: "Margin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Margin",
                table: "Order",
                newName: "Cost");
        }
    }
}
