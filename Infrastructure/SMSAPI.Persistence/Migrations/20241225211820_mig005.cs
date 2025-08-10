using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMSAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig005 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BuildQuality",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CargoSpace",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChassisMaterial",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Doors",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FuelEffiency",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FuelType",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Height",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HorsePower",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Hundread2TwoHundread",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Kw",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Length",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PassengerCapacity",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TopSpeed",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Torque",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TractionSystem",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Transmission",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TurboType",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Width",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zero2Hundread",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuildQuality",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CargoSpace",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ChassisMaterial",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Doors",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FuelEffiency",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FuelType",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "HorsePower",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Hundread2TwoHundread",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Kw",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PassengerCapacity",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TopSpeed",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Torque",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TractionSystem",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Transmission",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TurboType",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Zero2Hundread",
                table: "Products");
        }
    }
}
