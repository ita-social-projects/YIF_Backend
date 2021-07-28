using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class CascadeDeleteIsOffForSpecialtyIdForLector : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lectors_Specialties_SpecialtyId",
                table: "Lectors");

            migrationBuilder.AddForeignKey(
                name: "FK_Lectors_Specialties_SpecialtyId",
                table: "Lectors",
                column: "SpecialtyId",
                principalTable: "Specialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lectors_Specialties_SpecialtyId",
                table: "Lectors");

            migrationBuilder.AddForeignKey(
                name: "FK_Lectors_Specialties_SpecialtyId",
                table: "Lectors",
                column: "SpecialtyId",
                principalTable: "Specialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
