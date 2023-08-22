using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_ImagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GuidName",
                table: "Image",
                newName: "Name");

            migrationBuilder.AddColumn<byte[]>(
                name: "Avatar",
                table: "Image",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Image");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Image",
                newName: "GuidName");
        }
    }
}
