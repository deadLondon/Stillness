using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stillness.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserModel12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureId",
                table: "User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PictureId",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
