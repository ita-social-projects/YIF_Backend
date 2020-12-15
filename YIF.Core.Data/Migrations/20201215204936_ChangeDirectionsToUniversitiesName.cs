using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class ChangeDirectionsToUniversitiesName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecialityToUniversities_Directions_DirectionId",
                table: "SpecialityToUniversities");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialityToUniversities_Universities_UniversityId",
                table: "SpecialityToUniversities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SpecialityToUniversities",
                table: "SpecialityToUniversities");

            migrationBuilder.RenameTable(
                name: "SpecialityToUniversities",
                newName: "DirectionsToUniversities");

            migrationBuilder.RenameIndex(
                name: "IX_SpecialityToUniversities_UniversityId",
                table: "DirectionsToUniversities",
                newName: "IX_DirectionsToUniversities_UniversityId");

            migrationBuilder.RenameIndex(
                name: "IX_SpecialityToUniversities_DirectionId",
                table: "DirectionsToUniversities",
                newName: "IX_DirectionsToUniversities_DirectionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DirectionsToUniversities",
                table: "DirectionsToUniversities",
                columns: new[] { "Id", "UniversityId", "DirectionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DirectionsToUniversities_Directions_DirectionId",
                table: "DirectionsToUniversities",
                column: "DirectionId",
                principalTable: "Directions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DirectionsToUniversities_Universities_UniversityId",
                table: "DirectionsToUniversities",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DirectionsToUniversities_Directions_DirectionId",
                table: "DirectionsToUniversities");

            migrationBuilder.DropForeignKey(
                name: "FK_DirectionsToUniversities_Universities_UniversityId",
                table: "DirectionsToUniversities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DirectionsToUniversities",
                table: "DirectionsToUniversities");

            migrationBuilder.RenameTable(
                name: "DirectionsToUniversities",
                newName: "SpecialityToUniversities");

            migrationBuilder.RenameIndex(
                name: "IX_DirectionsToUniversities_UniversityId",
                table: "SpecialityToUniversities",
                newName: "IX_SpecialityToUniversities_UniversityId");

            migrationBuilder.RenameIndex(
                name: "IX_DirectionsToUniversities_DirectionId",
                table: "SpecialityToUniversities",
                newName: "IX_SpecialityToUniversities_DirectionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpecialityToUniversities",
                table: "SpecialityToUniversities",
                columns: new[] { "Id", "UniversityId", "DirectionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialityToUniversities_Directions_DirectionId",
                table: "SpecialityToUniversities",
                column: "DirectionId",
                principalTable: "Directions",
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
    }
}
