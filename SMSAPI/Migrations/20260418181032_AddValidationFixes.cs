using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmsWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddValidationFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "43mtp8kr-b7a3-d725-3c8g-75s8ba8uf529", "0f9fe22a-0347-4fb9-9891-fb801308849b" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "639de03f-7876-4fff-96ec-37f8bd3bf180", "3cf4fbe9-1a45-4a9a-9427-f717d574d5ac" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "78udf5dc-d9s5-h581-6u5g-16k3dt3sd762",
                column: "Name",
                value: "StockManager");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "78udf5dc-d9s5-h581-6u5g-16k3dt3sd762",
                column: "Name",
                value: "Stock Manager");

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "43mtp8kr-b7a3-d725-3c8g-75s8ba8uf529", "0f9fe22a-0347-4fb9-9891-fb801308849b" },
                    { "639de03f-7876-4fff-96ec-37f8bd3bf180", "3cf4fbe9-1a45-4a9a-9427-f717d574d5ac" }
                });
        }
    }
}
