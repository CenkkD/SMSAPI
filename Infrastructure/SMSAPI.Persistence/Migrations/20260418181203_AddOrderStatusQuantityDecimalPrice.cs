using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMSAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderStatusQuantityDecimalPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Products_Orders",
                table: "Products_Orders");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Products_Orders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Products_Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Products_Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products_Orders",
                table: "Products_Orders",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Orders_ProductId_OrderId",
                table: "Products_Orders",
                columns: new[] { "ProductId", "OrderId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Products_Orders",
                table: "Products_Orders");

            migrationBuilder.DropIndex(
                name: "IX_Products_Orders_ProductId_OrderId",
                table: "Products_Orders");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Products_Orders");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Products_Orders");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Products_Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<long>(
                name: "Price",
                table: "Products",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products_Orders",
                table: "Products_Orders",
                columns: new[] { "ProductId", "OrderId" });
        }
    }
}
