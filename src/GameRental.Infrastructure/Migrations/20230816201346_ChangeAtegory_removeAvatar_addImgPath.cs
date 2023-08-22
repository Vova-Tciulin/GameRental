using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAtegory_removeAvatar_addImgPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "ImgPath",
                table: "ProductCategory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "07c3a287-46f7-4b3b-92ad-9c3f401d248b", null, "Admin", "ADMIN" },
                    { "b42a950d-3dc5-4245-830b-ad5cbec9952e", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "07c3a287-46f7-4b3b-92ad-9c3f401d248b");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "b42a950d-3dc5-4245-830b-ad5cbec9952e");

            migrationBuilder.DropColumn(
                name: "ImgPath",
                table: "ProductCategory");

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
    }
}
