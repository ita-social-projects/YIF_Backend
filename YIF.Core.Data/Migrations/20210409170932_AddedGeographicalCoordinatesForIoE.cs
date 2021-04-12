using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class AddedGeographicalCoordinatesForIoE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Lat",
                table: "InstitutionOfEducations",
                nullable:true);

            migrationBuilder.AddColumn<float>(
                name: "Lon",
                table: "InstitutionOfEducations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lat",
                table: "InstitutionOfEducations");

            migrationBuilder.DropColumn(
                name: "Lon",
                table: "InstitutionOfEducations");
        }
    }
}
