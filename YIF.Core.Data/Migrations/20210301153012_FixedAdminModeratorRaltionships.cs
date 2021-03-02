using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class FixedAdminModeratorRaltionships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UniversityAdmins_Universities_UniversityId",
                table: "UniversityAdmins");

            migrationBuilder.DropForeignKey(
                name: "FK_UniversityModerators_Universities_UniversityId",
                table: "UniversityModerators");

            migrationBuilder.DropIndex(
                name: "IX_UniversityModerators_AdminId",
                table: "UniversityModerators");

            migrationBuilder.DropIndex(
                name: "IX_UniversityModerators_UniversityId",
                table: "UniversityModerators");

            migrationBuilder.DropColumn(
                name: "UniversityId",
                table: "UniversityModerators");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UniversityAdmins",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UniversityModerators_AdminId",
                table: "UniversityModerators",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_UniversityAdmins_UserId",
                table: "UniversityAdmins",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UniversityAdmins_Universities_UniversityId",
                table: "UniversityAdmins",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UniversityAdmins_AspNetUsers_UserId",
                table: "UniversityAdmins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UniversityAdmins_Universities_UniversityId",
                table: "UniversityAdmins");

            migrationBuilder.DropForeignKey(
                name: "FK_UniversityAdmins_AspNetUsers_UserId",
                table: "UniversityAdmins");

            migrationBuilder.DropIndex(
                name: "IX_UniversityModerators_AdminId",
                table: "UniversityModerators");

            migrationBuilder.DropIndex(
                name: "IX_UniversityAdmins_UserId",
                table: "UniversityAdmins");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UniversityAdmins");

            migrationBuilder.AddColumn<string>(
                name: "UniversityId",
                table: "UniversityModerators",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UniversityModerators_AdminId",
                table: "UniversityModerators",
                column: "AdminId",
                unique: true,
                filter: "[AdminId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UniversityModerators_UniversityId",
                table: "UniversityModerators",
                column: "UniversityId");

            migrationBuilder.AddForeignKey(
                name: "FK_UniversityAdmins_Universities_UniversityId",
                table: "UniversityAdmins",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UniversityModerators_Universities_UniversityId",
                table: "UniversityModerators",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
