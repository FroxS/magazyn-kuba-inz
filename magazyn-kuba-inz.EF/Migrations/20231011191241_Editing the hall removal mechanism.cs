using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace magazyn_kuba_inz.EF.Migrations
{
    /// <inheritdoc />
    public partial class Editingthehallremovalmechanism : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rack_Hall_ID_Hall",
                table: "Rack");

            migrationBuilder.AddForeignKey(
                name: "FK_Rack_Hall_ID_Hall",
                table: "Rack",
                column: "ID_Hall",
                principalTable: "Hall",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rack_Hall_ID_Hall",
                table: "Rack");

            migrationBuilder.AddForeignKey(
                name: "FK_Rack_Hall_ID_Hall",
                table: "Rack",
                column: "ID_Hall",
                principalTable: "Hall",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
