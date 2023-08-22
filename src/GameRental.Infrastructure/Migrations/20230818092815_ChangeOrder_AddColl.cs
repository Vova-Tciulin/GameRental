using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOrder_AddColl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "036d39e2-60a5-4194-b49d-6291716c4294");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "30d232bf-e48e-4a58-b4da-9e7c9c2362c4");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Order",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4c93fbd8-9c7e-4e17-b93e-d450f39a5cf7", null, "User", "USER" },
                    { "c12f7ec1-c50f-4ba7-897c-1043e662bef5", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "4c93fbd8-9c7e-4e17-b93e-d450f39a5cf7");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "c12f7ec1-c50f-4ba7-897c-1043e662bef5");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Order");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "036d39e2-60a5-4194-b49d-6291716c4294", null, "Admin", "ADMIN" },
                    { "30d232bf-e48e-4a58-b4da-9e7c9c2362c4", null, "User", "USER" }
                });
        }
    }
}
