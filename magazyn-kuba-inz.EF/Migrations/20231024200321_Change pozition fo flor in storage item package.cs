using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.EF.Migrations
{
    /// <inheritdoc />
    public partial class Changepozitionfoflorinstorageitempackage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "StorageItemPackage");

            migrationBuilder.AddColumn<int>(
                name: "Flor",
                table: "StorageItemPackage",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Flor",
                table: "StorageItemPackage");

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "StorageItemPackage",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
