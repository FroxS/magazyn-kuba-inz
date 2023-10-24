using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.EF.Migrations
{
    /// <inheritdoc />
    public partial class allowsetIdordertostorageitem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StorageItem_ID_OrderItem",
                table: "StorageItem");

            migrationBuilder.AlterColumn<Guid>(
                name: "ID_OrderItem",
                table: "StorageItem",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_StorageItem_ID_OrderItem",
                table: "StorageItem",
                column: "ID_OrderItem",
                unique: true,
                filter: "[ID_OrderItem] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StorageItem_ID_OrderItem",
                table: "StorageItem");

            migrationBuilder.AlterColumn<Guid>(
                name: "ID_OrderItem",
                table: "StorageItem",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StorageItem_ID_OrderItem",
                table: "StorageItem",
                column: "ID_OrderItem",
                unique: true);
        }
    }
}
