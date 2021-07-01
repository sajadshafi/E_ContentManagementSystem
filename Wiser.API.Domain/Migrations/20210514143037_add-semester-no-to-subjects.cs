using Microsoft.EntityFrameworkCore.Migrations;

namespace Wiser.API.Entities.Migrations
{
    public partial class addsemesternotosubjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SemesterNo",
                table: "Subjects",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SemesterNo",
                table: "Subjects");
        }
    }
}
