using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeImage_removeAvatar_addImgPat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "a648281f-d3da-43ee-8590-b1a98ed8c219");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "bb6d91b8-d78b-46aa-a2b1-33ba077f528e");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "036d39e2-60a5-4194-b49d-6291716c4294", null, "Admin", "ADMIN" },
                    { "30d232bf-e48e-4a58-b4da-9e7c9c2362c4", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "036d39e2-60a5-4194-b49d-6291716c4294");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "30d232bf-e48e-4a58-b4da-9e7c9c2362c4");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a648281f-d3da-43ee-8590-b1a98ed8c219", null, "Admin", "ADMIN" },
                    { "bb6d91b8-d78b-46aa-a2b1-33ba077f528e", null, "User", "USER" }
                });
        }
    }
}
