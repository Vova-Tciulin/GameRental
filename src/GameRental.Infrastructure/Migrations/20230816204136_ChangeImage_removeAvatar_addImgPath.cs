using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeImage_removeAvatar_addImgPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "Avatar",
                table: "Image");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Image",
                newName: "ImgName");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a648281f-d3da-43ee-8590-b1a98ed8c219", null, "Admin", "ADMIN" },
                    { "bb6d91b8-d78b-46aa-a2b1-33ba077f528e", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "a648281f-d3da-43ee-8590-b1a98ed8c219");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "bb6d91b8-d78b-46aa-a2b1-33ba077f528e");

            migrationBuilder.RenameColumn(
                name: "ImgName",
                table: "Image",
                newName: "Name");

            migrationBuilder.AddColumn<byte[]>(
                name: "Avatar",
                table: "Image",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "07c3a287-46f7-4b3b-92ad-9c3f401d248b", null, "Admin", "ADMIN" },
                    { "b42a950d-3dc5-4245-830b-ad5cbec9952e", null, "User", "USER" }
                });
        }
    }
}
