using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Relaxinema.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotosForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoPublicId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPublicId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "AspNetUsers");
        }
    }
}
