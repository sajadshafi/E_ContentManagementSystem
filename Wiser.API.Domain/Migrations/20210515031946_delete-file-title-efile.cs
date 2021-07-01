using Microsoft.EntityFrameworkCore.Migrations;

namespace Wiser.API.Entities.Migrations
{
    public partial class deletefiletitleefile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileTitle",
                table: "EFiles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileTitle",
                table: "EFiles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
