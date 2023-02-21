using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace magazyn_kuba_inz.EF.Migrations
{
    /// <inheritdoc />
    public partial class Firstmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rack",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Corridor = table.Column<int>(type: "int", nullable: false),
                    Flors = table.Column<int>(type: "int", nullable: false),
                    Width = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    Height = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    Depth = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    Direction = table.Column<int>(type: "int", nullable: false),
                    Space_amount = table.Column<long>(type: "bigint", nullable: false, defaultValue: 1L)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rack", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StorageUnit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxWeight = table.Column<double>(type: "float(3)", precision: 3, nullable: false),
                    MaxHeight = table.Column<double>(type: "float(3)", precision: 3, nullable: false),
                    MaxWidth = table.Column<double>(type: "float(3)", precision: 3, nullable: false),
                    MaxDepth = table.Column<double>(type: "float(3)", precision: 3, nullable: false),
                    RackUnitCapacity = table.Column<double>(type: "float(3)", maxLength: 1, precision: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageUnit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Price = table.Column<double>(type: "float(2)", precision: 2, nullable: false, defaultValue: 0.0),
                    Count = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Id_Group = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_ProductGroup_Id_Group",
                        column: x => x.Id_Group,
                        principalTable: "ProductGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RackPosition",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id_Rack = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Flor = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    Position = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RackPosition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RackPosition_Rack_Id_Rack",
                        column: x => x.Id_Rack,
                        principalTable: "Rack",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Id_User = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 2, 21, 22, 45, 2, 114, DateTimeKind.Local).AddTicks(1854)),
                    RealizationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Users_Id_User",
                        column: x => x.Id_User,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StorageItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id_StorageUnit = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Id_RackPosition = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StorageItem_RackPosition_Id_RackPosition",
                        column: x => x.Id_RackPosition,
                        principalTable: "RackPosition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StorageItem_StorageUnit_Id_StorageUnit",
                        column: x => x.Id_StorageUnit,
                        principalTable: "StorageUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderElement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id_Order = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Id_StorageItem = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderElement_Order_Id_Order",
                        column: x => x.Id_Order,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderElement_StorageItem_Id_StorageItem",
                        column: x => x.Id_StorageItem,
                        principalTable: "StorageItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StorageItemCollection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id_Product = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id_StorageItem = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageItemCollection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StorageItemCollection_Product_Id_Product",
                        column: x => x.Id_Product,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StorageItemCollection_StorageItem_Id_StorageItem",
                        column: x => x.Id_StorageItem,
                        principalTable: "StorageItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_Id_User",
                table: "Order",
                column: "Id_User");

            migrationBuilder.CreateIndex(
                name: "IX_OrderElement_Id_Order",
                table: "OrderElement",
                column: "Id_Order");

            migrationBuilder.CreateIndex(
                name: "IX_OrderElement_Id_StorageItem",
                table: "OrderElement",
                column: "Id_StorageItem");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Id_Group",
                table: "Product",
                column: "Id_Group");

            migrationBuilder.CreateIndex(
                name: "IX_RackPosition_Id_Rack",
                table: "RackPosition",
                column: "Id_Rack");

            migrationBuilder.CreateIndex(
                name: "IX_StorageItem_Id_RackPosition",
                table: "StorageItem",
                column: "Id_RackPosition");

            migrationBuilder.CreateIndex(
                name: "IX_StorageItem_Id_StorageUnit",
                table: "StorageItem",
                column: "Id_StorageUnit");

            migrationBuilder.CreateIndex(
                name: "IX_StorageItemCollection_Id_Product",
                table: "StorageItemCollection",
                column: "Id_Product");

            migrationBuilder.CreateIndex(
                name: "IX_StorageItemCollection_Id_StorageItem",
                table: "StorageItemCollection",
                column: "Id_StorageItem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderElement");

            migrationBuilder.DropTable(
                name: "StorageItemCollection");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "StorageItem");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ProductGroup");

            migrationBuilder.DropTable(
                name: "RackPosition");

            migrationBuilder.DropTable(
                name: "StorageUnit");

            migrationBuilder.DropTable(
                name: "Rack");
        }
    }
}
