using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class AdjustedName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Decription",
                table: "SpecialtyInUniversityDescriptions");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SpecialtyInUniversityDescriptions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "SpecialtyInUniversityDescriptions");

            migrationBuilder.AddColumn<string>(
                name: "Decription",
                table: "SpecialtyInUniversityDescriptions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
