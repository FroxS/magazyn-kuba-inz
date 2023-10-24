using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.EF.Migrations
{
    /// <inheritdoc />
    public partial class ChangelinkbeetwenstorageItemandWarehouseItemandProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StorageItem_Product_ID_Product",
                table: "StorageItem");

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItem_WareHouseItem_ID_Product",
                table: "StorageItem",
                column: "ID_Product",
                principalTable: "WareHouseItem",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StorageItem_WareHouseItem_ID_Product",
                table: "StorageItem");

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItem_Product_ID_Product",
                table: "StorageItem",
                column: "ID_Product",
                principalTable: "Product",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
