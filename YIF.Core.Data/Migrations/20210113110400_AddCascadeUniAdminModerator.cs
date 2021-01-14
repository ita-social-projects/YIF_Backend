using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class AddCascadeUniAdminModerator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UniversityAdmins_Universities_UniversityId",
                table: "UniversityAdmins");

            migrationBuilder.DropForeignKey(
                name: "FK_UniversityModerators_UniversityAdmins_AdminId",
                table: "UniversityModerators");

            migrationBuilder.AddForeignKey(
                name: "FK_UniversityAdmins_Universities_UniversityId",
                table: "UniversityAdmins",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UniversityModerators_UniversityAdmins_AdminId",
                table: "UniversityModerators",
                column: "AdminId",
                principalTable: "UniversityAdmins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UniversityAdmins_Universities_UniversityId",
                table: "UniversityAdmins");

            migrationBuilder.DropForeignKey(
                name: "FK_UniversityModerators_UniversityAdmins_AdminId",
                table: "UniversityModerators");

            migrationBuilder.AddForeignKey(
                name: "FK_UniversityAdmins_Universities_UniversityId",
                table: "UniversityAdmins",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UniversityModerators_UniversityAdmins_AdminId",
                table: "UniversityModerators",
                column: "AdminId",
                principalTable: "UniversityAdmins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
