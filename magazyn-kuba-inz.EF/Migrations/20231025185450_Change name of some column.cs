using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.EF.Migrations
{
    /// <inheritdoc />
    public partial class Changenameofsomecolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StorageItem_StorageItemPackage_ID_StorageItem",
                table: "StorageItem");

            migrationBuilder.DropForeignKey(
                name: "FK_StorageItem_WareHouseItem_ID_Product",
                table: "StorageItem");

            migrationBuilder.RenameColumn(
                name: "ID_StorageItem",
                table: "StorageItem",
                newName: "ID_Package");

            migrationBuilder.RenameColumn(
                name: "ID_Product",
                table: "StorageItem",
                newName: "ID_Item");

            migrationBuilder.RenameIndex(
                name: "IX_StorageItem_ID_StorageItem",
                table: "StorageItem",
                newName: "IX_StorageItem_ID_Package");

            migrationBuilder.RenameIndex(
                name: "IX_StorageItem_ID_Product",
                table: "StorageItem",
                newName: "IX_StorageItem_ID_Item");

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItem_StorageItemPackage_ID_Package",
                table: "StorageItem",
                column: "ID_Package",
                principalTable: "StorageItemPackage",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItem_WareHouseItem_ID_Item",
                table: "StorageItem",
                column: "ID_Item",
                principalTable: "WareHouseItem",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StorageItem_StorageItemPackage_ID_Package",
                table: "StorageItem");

            migrationBuilder.DropForeignKey(
                name: "FK_StorageItem_WareHouseItem_ID_Item",
                table: "StorageItem");

            migrationBuilder.RenameColumn(
                name: "ID_Package",
                table: "StorageItem",
                newName: "ID_StorageItem");

            migrationBuilder.RenameColumn(
                name: "ID_Item",
                table: "StorageItem",
                newName: "ID_Product");

            migrationBuilder.RenameIndex(
                name: "IX_StorageItem_ID_Package",
                table: "StorageItem",
                newName: "IX_StorageItem_ID_StorageItem");

            migrationBuilder.RenameIndex(
                name: "IX_StorageItem_ID_Item",
                table: "StorageItem",
                newName: "IX_StorageItem_ID_Product");

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItem_StorageItemPackage_ID_StorageItem",
                table: "StorageItem",
                column: "ID_StorageItem",
                principalTable: "StorageItemPackage",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItem_WareHouseItem_ID_Product",
                table: "StorageItem",
                column: "ID_Product",
                principalTable: "WareHouseItem",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
