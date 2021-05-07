using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class AddedIsBannedFieldToIoE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isBanned",
                table: "InstitutionOfEducations",
                newName: "IsBanned");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsBanned",
                table: "InstitutionOfEducations",
                newName: "isBanned");
        }
    }
}
