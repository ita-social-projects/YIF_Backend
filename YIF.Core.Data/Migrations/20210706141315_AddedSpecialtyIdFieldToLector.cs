using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class AddedSpecialtyIdFieldToLector : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SpecialtyId",
                table: "Lectors",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lectors_SpecialtyId",
                table: "Lectors",
                column: "SpecialtyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lectors_Specialties_SpecialtyId",
                table: "Lectors",
                column: "SpecialtyId",
                principalTable: "Specialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lectors_Specialties_SpecialtyId",
                table: "Lectors");

            migrationBuilder.DropIndex(
                name: "IX_Lectors_SpecialtyId",
                table: "Lectors");

            migrationBuilder.DropColumn(
                name: "SpecialtyId",
                table: "Lectors");
        }
    }
}
