using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class AddedCodesForSpecialtyAndDirection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Directions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Specialties",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Directions");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Specialties");
        }
    }
}
