using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeConfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "4c93fbd8-9c7e-4e17-b93e-d450f39a5cf7");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "c12f7ec1-c50f-4ba7-897c-1043e662bef5");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProductCategory",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "c20a8094-d15e-40e0-abe0-02407ccbb813", null, "User", "USER" },
                    { "d5049a4a-5d15-4b89-a189-26d9205b812b", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "c20a8094-d15e-40e0-abe0-02407ccbb813");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "d5049a4a-5d15-4b89-a189-26d9205b812b");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProductCategory",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4c93fbd8-9c7e-4e17-b93e-d450f39a5cf7", null, "User", "USER" },
                    { "c12f7ec1-c50f-4ba7-897c-1043e662bef5", null, "Admin", "ADMIN" }
                });
        }
    }
}
