using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.EF.Migrations
{
    /// <inheritdoc />
    public partial class added_hals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ID_Hall",
                table: "Rack",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Hall",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Lp = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hall", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rack_ID_Hall",
                table: "Rack",
                column: "ID_Hall");

            migrationBuilder.AddForeignKey(
                name: "FK_Rack_Hall_ID_Hall",
                table: "Rack",
                column: "ID_Hall",
                principalTable: "Hall",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rack_Hall_ID_Hall",
                table: "Rack");

            migrationBuilder.DropTable(
                name: "Hall");

            migrationBuilder.DropIndex(
                name: "IX_Rack_ID_Hall",
                table: "Rack");

            migrationBuilder.DropColumn(
                name: "ID_Hall",
                table: "Rack");
        }
    }
}
