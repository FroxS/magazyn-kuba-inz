using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.EF.Migrations
{
    /// <inheritdoc />
    public partial class Init_storage_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rack",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Corridor = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Flors = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Width = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    Heigth = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    Deepth = table.Column<double>(type: "float", nullable: false, defaultValue: 1.0),
                    Direction = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AmountSpace = table.Column<int>(type: "int", nullable: false, defaultValue: 2),
                    Lp = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rack", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "StorageUnit",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxWeight = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    MaxHeight = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    MaxWidth = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    MaxDepth = table.Column<double>(type: "float", nullable: false, defaultValue: 1.0),
                    Lp = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageUnit", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "StorageItem",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ID_StorageUnit = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ID_Rack = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Lp = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_StorageItem_Rack_ID_Rack",
                        column: x => x.ID_Rack,
                        principalTable: "Rack",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StorageItem_StorageUnit_ID_StorageUnit",
                        column: x => x.ID_StorageUnit,
                        principalTable: "StorageUnit",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StorageItemCollection",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ID_StorageItem = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ID_Product = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Lp = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageItemCollection", x => x.ID);
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
                name: "IX_StorageItem_ID_Rack",
                table: "StorageItem",
                column: "ID_Rack");

            migrationBuilder.CreateIndex(
                name: "IX_StorageItem_ID_StorageUnit",
                table: "StorageItem",
                column: "ID_StorageUnit");

            migrationBuilder.CreateIndex(
                name: "IX_StorageItemCollection_ID_Product",
                table: "StorageItemCollection",
                column: "ID_Product");

            migrationBuilder.CreateIndex(
                name: "IX_StorageItemCollection_ID_StorageItem",
                table: "StorageItemCollection",
                column: "ID_StorageItem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StorageItemCollection");

            migrationBuilder.DropTable(
                name: "StorageItem");

            migrationBuilder.DropTable(
                name: "Rack");

            migrationBuilder.DropTable(
                name: "StorageUnit");
        }
    }
}
