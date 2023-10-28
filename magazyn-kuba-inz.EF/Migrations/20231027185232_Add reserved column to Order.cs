using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddreservedcolumntoOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Reserved",
                table: "Order",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reserved",
                table: "Order");
        }
    }
}
