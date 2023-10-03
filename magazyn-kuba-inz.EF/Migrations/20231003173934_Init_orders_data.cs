using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace magazyn_kuba_inz.EF.Migrations
{
    /// <inheritdoc />
    public partial class Init_orders_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "StorageUnit",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "ID_OrderItem",
                table: "StorageItemCollection",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Position",
                table: "StorageItem",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ID_User = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RealizationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    Lp = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Order_User_ID_User",
                        column: x => x.ID_User,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderProduct",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ID_Order = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ID_Product = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ID_StorageItem = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Lp = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProduct", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrderProduct_Order_ID_Order",
                        column: x => x.ID_Order,
                        principalTable: "Order",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProduct_Product_ID_Product",
                        column: x => x.ID_Product,
                        principalTable: "Product",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StorageItemCollection_ID_OrderItem",
                table: "StorageItemCollection",
                column: "ID_OrderItem",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_ID_User",
                table: "Order",
                column: "ID_User");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProduct_ID_Order",
                table: "OrderProduct",
                column: "ID_Order");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProduct_ID_Product",
                table: "OrderProduct",
                column: "ID_Product");

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItemCollection_OrderProduct_ID_OrderItem",
                table: "StorageItemCollection",
                column: "ID_OrderItem",
                principalTable: "OrderProduct",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StorageItemCollection_OrderProduct_ID_OrderItem",
                table: "StorageItemCollection");

            migrationBuilder.DropTable(
                name: "OrderProduct");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropIndex(
                name: "IX_StorageItemCollection_ID_OrderItem",
                table: "StorageItemCollection");

            migrationBuilder.DropColumn(
                name: "ID_OrderItem",
                table: "StorageItemCollection");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "StorageUnit",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Position",
                table: "StorageItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
