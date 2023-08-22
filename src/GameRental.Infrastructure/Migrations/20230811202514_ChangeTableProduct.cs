using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTableProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "998215b8-fc7b-4b71-bf79-cce97db78859");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "a0c03362-879d-4a02-84d5-cd18db5ca659");

            migrationBuilder.AddColumn<int>(
                name: "TransitTime",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Translate",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0d3a016d-f46f-487d-bf4e-0d787d9ebee6", null, "Admin", "ADMIN" },
                    { "6204612e-98d2-4999-8b95-88f347ac54b9", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "0d3a016d-f46f-487d-bf4e-0d787d9ebee6");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "6204612e-98d2-4999-8b95-88f347ac54b9");

            migrationBuilder.DropColumn(
                name: "TransitTime",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Translate",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Product");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "998215b8-fc7b-4b71-bf79-cce97db78859", null, "User", "USER" },
                    { "a0c03362-879d-4a02-84d5-cd18db5ca659", null, "Admin", "ADMIN" }
                });
        }
    }
}
