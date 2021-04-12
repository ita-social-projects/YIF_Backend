using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class AddedIoEToGraduate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InstitutionOfEducationsToGraduates",
                columns: table => new
                {
                    GraduateId = table.Column<string>(nullable: false),
                    InstitutionOfEducationId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionOfEducationsToGraduates", x => new { x.InstitutionOfEducationId, x.GraduateId });
                    table.ForeignKey(
                        name: "FK_InstitutionOfEducationsToGraduates_Graduates_GraduateId",
                        column: x => x.GraduateId,
                        principalTable: "Graduates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstitutionOfEducationsToGraduates_InstitutionOfEducations_InstitutionOfEducationId",
                        column: x => x.InstitutionOfEducationId,
                        principalTable: "InstitutionOfEducations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionOfEducationsToGraduates_GraduateId",
                table: "InstitutionOfEducationsToGraduates",
                column: "GraduateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstitutionOfEducationsToGraduates");
        }
    }
}
