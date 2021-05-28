using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class ChangeEntityLectureToLector : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lectures_InstitutionOfEducations_InstitutionOfEducationId",
                table: "Lectures");

            migrationBuilder.DropForeignKey(
                name: "FK_Lectures_AspNetUsers_UserId",
                table: "Lectures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lectures",
                table: "Lectures");

            migrationBuilder.RenameTable(
                name: "Lectures",
                newName: "Lector");

            migrationBuilder.RenameIndex(
                name: "IX_Lectures_UserId",
                table: "Lector",
                newName: "IX_Lector_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Lectures_InstitutionOfEducationId",
                table: "Lector",
                newName: "IX_Lector_InstitutionOfEducationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lector",
                table: "Lector",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lector_InstitutionOfEducations_InstitutionOfEducationId",
                table: "Lector",
                column: "InstitutionOfEducationId",
                principalTable: "InstitutionOfEducations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lector_AspNetUsers_UserId",
                table: "Lector",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lector_InstitutionOfEducations_InstitutionOfEducationId",
                table: "Lector");

            migrationBuilder.DropForeignKey(
                name: "FK_Lector_AspNetUsers_UserId",
                table: "Lector");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lector",
                table: "Lector");

            migrationBuilder.RenameTable(
                name: "Lector",
                newName: "Lectures");

            migrationBuilder.RenameIndex(
                name: "IX_Lector_UserId",
                table: "Lectures",
                newName: "IX_Lectures_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Lector_InstitutionOfEducationId",
                table: "Lectures",
                newName: "IX_Lectures_InstitutionOfEducationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lectures",
                table: "Lectures",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lectures_InstitutionOfEducations_InstitutionOfEducationId",
                table: "Lectures",
                column: "InstitutionOfEducationId",
                principalTable: "InstitutionOfEducations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lectures_AspNetUsers_UserId",
                table: "Lectures",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
