using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.EF.Migrations
{
    /// <inheritdoc />
    public partial class removewarehouseitemfromdatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StorageItem_WareHouseItem_ID_Item",
                table: "StorageItem");

            migrationBuilder.DropTable(
                name: "WareHouseItem");

            migrationBuilder.DropColumn(
                name: "Reserved",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "ID_Item",
                table: "StorageItem",
                newName: "ID_State");

            migrationBuilder.RenameIndex(
                name: "IX_StorageItem_ID_Item",
                table: "StorageItem",
                newName: "IX_StorageItem_ID_State");

            migrationBuilder.AddColumn<Guid>(
                name: "ID_Product",
                table: "StorageItem",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_StorageItem_ID_Product",
                table: "StorageItem",
                column: "ID_Product");

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItem_ItemState_ID_State",
                table: "StorageItem",
                column: "ID_State",
                principalTable: "ItemState",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItem_Product_ID_Product",
                table: "StorageItem",
                column: "ID_Product",
                principalTable: "Product",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StorageItem_ItemState_ID_State",
                table: "StorageItem");

            migrationBuilder.DropForeignKey(
                name: "FK_StorageItem_Product_ID_Product",
                table: "StorageItem");

            migrationBuilder.DropIndex(
                name: "IX_StorageItem_ID_Product",
                table: "StorageItem");

            migrationBuilder.DropColumn(
                name: "ID_Product",
                table: "StorageItem");

            migrationBuilder.RenameColumn(
                name: "ID_State",
                table: "StorageItem",
                newName: "ID_Item");

            migrationBuilder.RenameIndex(
                name: "IX_StorageItem_ID_State",
                table: "StorageItem",
                newName: "IX_StorageItem_ID_Item");

            migrationBuilder.AddColumn<bool>(
                name: "Reserved",
                table: "Order",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "WareHouseItem",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ID_Product = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ID_State = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Lp = table.Column<long>(type: "bigint", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WareHouseItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WareHouseItem_ItemState_ID_State",
                        column: x => x.ID_State,
                        principalTable: "ItemState",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WareHouseItem_Product_ID_Product",
                        column: x => x.ID_Product,
                        principalTable: "Product",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WareHouseItem_ID_Product",
                table: "WareHouseItem",
                column: "ID_Product");

            migrationBuilder.CreateIndex(
                name: "IX_WareHouseItem_ID_State",
                table: "WareHouseItem",
                column: "ID_State");

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItem_WareHouseItem_ID_Item",
                table: "StorageItem",
                column: "ID_Item",
                principalTable: "WareHouseItem",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
