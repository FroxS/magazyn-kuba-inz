using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.EF.Migrations
{
    /// <inheritdoc />
    public partial class Packageinstorageitemcanbenull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StorageItem_StorageItemPackage_ID_Package",
                table: "StorageItem");

            migrationBuilder.AlterColumn<Guid>(
                name: "ID_Package",
                table: "StorageItem",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItem_StorageItemPackage_ID_Package",
                table: "StorageItem",
                column: "ID_Package",
                principalTable: "StorageItemPackage",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StorageItem_StorageItemPackage_ID_Package",
                table: "StorageItem");

            migrationBuilder.AlterColumn<Guid>(
                name: "ID_Package",
                table: "StorageItem",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItem_StorageItemPackage_ID_Package",
                table: "StorageItem",
                column: "ID_Package",
                principalTable: "StorageItemPackage",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
