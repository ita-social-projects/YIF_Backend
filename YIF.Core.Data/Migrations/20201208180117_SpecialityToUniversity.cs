using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class SpecialityToUniversity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecialityToUniversity_Specialities_SpecialityId",
                table: "SpecialityToUniversity");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialityToUniversity_Universities_UniversityId",
                table: "SpecialityToUniversity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SpecialityToUniversity",
                table: "SpecialityToUniversity");

            migrationBuilder.RenameTable(
                name: "SpecialityToUniversity",
                newName: "SpecialityToUniversities");

            migrationBuilder.RenameIndex(
                name: "IX_SpecialityToUniversity_UniversityId",
                table: "SpecialityToUniversities",
                newName: "IX_SpecialityToUniversities_UniversityId");

            migrationBuilder.RenameIndex(
                name: "IX_SpecialityToUniversity_SpecialityId",
                table: "SpecialityToUniversities",
                newName: "IX_SpecialityToUniversities_SpecialityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpecialityToUniversities",
                table: "SpecialityToUniversities",
                columns: new[] { "Id", "UniversityId", "SpecialityId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialityToUniversities_Specialities_SpecialityId",
                table: "SpecialityToUniversities",
                column: "SpecialityId",
                principalTable: "Specialities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialityToUniversities_Universities_UniversityId",
                table: "SpecialityToUniversities",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecialityToUniversities_Specialities_SpecialityId",
                table: "SpecialityToUniversities");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialityToUniversities_Universities_UniversityId",
                table: "SpecialityToUniversities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SpecialityToUniversities",
                table: "SpecialityToUniversities");

            migrationBuilder.RenameTable(
                name: "SpecialityToUniversities",
                newName: "SpecialityToUniversity");

            migrationBuilder.RenameIndex(
                name: "IX_SpecialityToUniversities_UniversityId",
                table: "SpecialityToUniversity",
                newName: "IX_SpecialityToUniversity_UniversityId");

            migrationBuilder.RenameIndex(
                name: "IX_SpecialityToUniversities_SpecialityId",
                table: "SpecialityToUniversity",
                newName: "IX_SpecialityToUniversity_SpecialityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpecialityToUniversity",
                table: "SpecialityToUniversity",
                columns: new[] { "Id", "UniversityId", "SpecialityId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialityToUniversity_Specialities_SpecialityId",
                table: "SpecialityToUniversity",
                column: "SpecialityId",
                principalTable: "Specialities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialityToUniversity_Universities_UniversityId",
                table: "SpecialityToUniversity",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
