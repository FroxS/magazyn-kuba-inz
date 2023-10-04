using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace magazyn_kuba_inz.EF.Migrations
{
    /// <inheritdoc />
    public partial class changetablename_ofstorageitems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StorageItem_Rack_ID_Rack",
                table: "StorageItem");

            migrationBuilder.DropForeignKey(
                name: "FK_StorageItem_StorageUnit_ID_StorageUnit",
                table: "StorageItem");

            migrationBuilder.DropTable(
                name: "StorageItemCollection");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "StorageItem");

            migrationBuilder.RenameColumn(
                name: "ID_StorageUnit",
                table: "StorageItem",
                newName: "ID_StorageItem");

            migrationBuilder.RenameColumn(
                name: "ID_Rack",
                table: "StorageItem",
                newName: "ID_Product");

            migrationBuilder.RenameIndex(
                name: "IX_StorageItem_ID_StorageUnit",
                table: "StorageItem",
                newName: "IX_StorageItem_ID_StorageItem");

            migrationBuilder.RenameIndex(
                name: "IX_StorageItem_ID_Rack",
                table: "StorageItem",
                newName: "IX_StorageItem_ID_Product");

            migrationBuilder.AddColumn<Guid>(
                name: "ID_OrderItem",
                table: "StorageItem",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "CanRealizeOrder",
                table: "ItemState",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "InWareHouse",
                table: "ItemState",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "StorageItemPackage",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ID_StorageUnit = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ID_Rack = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Lp = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageItemPackage", x => x.ID);
                    table.ForeignKey(
                        name: "FK_StorageItemPackage_Rack_ID_Rack",
                        column: x => x.ID_Rack,
                        principalTable: "Rack",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StorageItemPackage_StorageUnit_ID_StorageUnit",
                        column: x => x.ID_StorageUnit,
                        principalTable: "StorageUnit",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StorageItem_ID_OrderItem",
                table: "StorageItem",
                column: "ID_OrderItem",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StorageItemPackage_ID_Rack",
                table: "StorageItemPackage",
                column: "ID_Rack");

            migrationBuilder.CreateIndex(
                name: "IX_StorageItemPackage_ID_StorageUnit",
                table: "StorageItemPackage",
                column: "ID_StorageUnit");

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItem_OrderProduct_ID_OrderItem",
                table: "StorageItem",
                column: "ID_OrderItem",
                principalTable: "OrderProduct",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItem_Product_ID_Product",
                table: "StorageItem",
                column: "ID_Product",
                principalTable: "Product",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItem_StorageItemPackage_ID_StorageItem",
                table: "StorageItem",
                column: "ID_StorageItem",
                principalTable: "StorageItemPackage",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StorageItem_OrderProduct_ID_OrderItem",
                table: "StorageItem");

            migrationBuilder.DropForeignKey(
                name: "FK_StorageItem_Product_ID_Product",
                table: "StorageItem");

            migrationBuilder.DropForeignKey(
                name: "FK_StorageItem_StorageItemPackage_ID_StorageItem",
                table: "StorageItem");

            migrationBuilder.DropTable(
                name: "StorageItemPackage");

            migrationBuilder.DropIndex(
                name: "IX_StorageItem_ID_OrderItem",
                table: "StorageItem");

            migrationBuilder.DropColumn(
                name: "ID_OrderItem",
                table: "StorageItem");

            migrationBuilder.DropColumn(
                name: "CanRealizeOrder",
                table: "ItemState");

            migrationBuilder.DropColumn(
                name: "InWareHouse",
                table: "ItemState");

            migrationBuilder.RenameColumn(
                name: "ID_StorageItem",
                table: "StorageItem",
                newName: "ID_StorageUnit");

            migrationBuilder.RenameColumn(
                name: "ID_Product",
                table: "StorageItem",
                newName: "ID_Rack");

            migrationBuilder.RenameIndex(
                name: "IX_StorageItem_ID_StorageItem",
                table: "StorageItem",
                newName: "IX_StorageItem_ID_StorageUnit");

            migrationBuilder.RenameIndex(
                name: "IX_StorageItem_ID_Product",
                table: "StorageItem",
                newName: "IX_StorageItem_ID_Rack");

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "StorageItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StorageItemCollection",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ID_OrderItem = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ID_Product = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ID_StorageItem = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Lp = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageItemCollection", x => x.ID);
                    table.ForeignKey(
                        name: "FK_StorageItemCollection_OrderProduct_ID_OrderItem",
                        column: x => x.ID_OrderItem,
                        principalTable: "OrderProduct",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StorageItemCollection_Product_ID_Product",
                        column: x => x.ID_Product,
                        principalTable: "Product",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StorageItemCollection_StorageItem_ID_StorageItem",
                        column: x => x.ID_StorageItem,
                        principalTable: "StorageItem",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StorageItemCollection_ID_OrderItem",
                table: "StorageItemCollection",
                column: "ID_OrderItem",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StorageItemCollection_ID_Product",
                table: "StorageItemCollection",
                column: "ID_Product");

            migrationBuilder.CreateIndex(
                name: "IX_StorageItemCollection_ID_StorageItem",
                table: "StorageItemCollection",
                column: "ID_StorageItem");

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItem_Rack_ID_Rack",
                table: "StorageItem",
                column: "ID_Rack",
                principalTable: "Rack",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItem_StorageUnit_ID_StorageUnit",
                table: "StorageItem",
                column: "ID_StorageUnit",
                principalTable: "StorageUnit",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
