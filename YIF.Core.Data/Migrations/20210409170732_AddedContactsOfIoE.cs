using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class AddedContactsOfIoE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                table: "InstitutionOfEducations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Site",
                table: "InstitutionOfEducations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "InstitutionOfEducations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "InstitutionOfEducations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "InstitutionOfEducations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abbreviation",
                table: "InstitutionOfEducations");

            migrationBuilder.DropColumn(
                name: "Site",
                table: "InstitutionOfEducations");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "InstitutionOfEducations");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "InstitutionOfEducations");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "InstitutionOfEducations");
        }
    }
}
