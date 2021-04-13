using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class AddedSpecialtyToIoEToGraduate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpecialtyToInstitutionOfEducationToGraduates",
                columns: table => new
                {
                    SpecialtyId = table.Column<string>(nullable: false),
                    InstitutionOfEducationId = table.Column<string>(nullable: false),
                    GraduateId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialtyToInstitutionOfEducationToGraduates", x => new { x.SpecialtyId, x.InstitutionOfEducationId, x.GraduateId });
                    table.ForeignKey(
                        name: "FK_SpecialtyToInstitutionOfEducationToGraduates_Graduates_GraduateId",
                        column: x => x.GraduateId,
                        principalTable: "Graduates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecialtyToInstitutionOfEducationToGraduates_InstitutionOfEducations_InstitutionOfEducationId",
                        column: x => x.InstitutionOfEducationId,
                        principalTable: "InstitutionOfEducations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecialtyToInstitutionOfEducationToGraduates_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpecialtyToInstitutionOfEducationToGraduates_GraduateId",
                table: "SpecialtyToInstitutionOfEducationToGraduates",
                column: "GraduateId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialtyToInstitutionOfEducationToGraduates_InstitutionOfEducationId",
                table: "SpecialtyToInstitutionOfEducationToGraduates",
                column: "InstitutionOfEducationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpecialtyToInstitutionOfEducationToGraduates");
        }
    }
}
