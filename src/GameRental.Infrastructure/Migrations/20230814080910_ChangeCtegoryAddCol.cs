using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCtegoryAddCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "0d3a016d-f46f-487d-bf4e-0d787d9ebee6");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "6204612e-98d2-4999-8b95-88f347ac54b9");

            migrationBuilder.AddColumn<byte[]>(
                name: "Avatar",
                table: "ProductCategory",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "03519d89-9c2a-483e-8abf-67af2909252b", null, "User", "USER" },
                    { "fb5cec1b-acfd-49ca-89c1-fd47dbeb76e4", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "03519d89-9c2a-483e-8abf-67af2909252b");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "fb5cec1b-acfd-49ca-89c1-fd47dbeb76e4");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "ProductCategory");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0d3a016d-f46f-487d-bf4e-0d787d9ebee6", null, "Admin", "ADMIN" },
                    { "6204612e-98d2-4999-8b95-88f347ac54b9", null, "User", "USER" }
                });
        }
    }
}
