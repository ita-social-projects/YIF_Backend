using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class AddSpecialtyToUniversityToGraduate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpecialtyToUniversityToGraduates",
                columns: table => new
                {
                    SpecialtyId = table.Column<string>(nullable: false),
                    UniversityId = table.Column<string>(nullable: false),
                    GraduateId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialtyToUniversityToGraduates", x => new { x.SpecialtyId, x.UniversityId, x.GraduateId });
                    table.ForeignKey(
                        name: "FK_SpecialtyToUniversityToGraduates_Graduates_GraduateId",
                        column: x => x.GraduateId,
                        principalTable: "Graduates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecialtyToUniversityToGraduates_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecialtyToUniversityToGraduates_Universities_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "Universities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpecialtyToUniversityToGraduates_GraduateId",
                table: "SpecialtyToUniversityToGraduates",
                column: "GraduateId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialtyToUniversityToGraduates_UniversityId",
                table: "SpecialtyToUniversityToGraduates",
                column: "UniversityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpecialtyToUniversityToGraduates");
        }
    }
}
