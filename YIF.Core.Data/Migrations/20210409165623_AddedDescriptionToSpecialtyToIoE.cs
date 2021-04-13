using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class AddedDescriptionToSpecialtyToIoE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpecialtyToIoEDescriptions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SpecialtyToInstitutionOfEducationId = table.Column<string>(nullable: true),
                    PaymentForm = table.Column<string>(nullable: false),
                    EducationForm = table.Column<string>(nullable: false),
                    EducationalProgramLink = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialtyToIoEDescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialtyToIoEDescriptions_SpecialtyToInstitutionOfEducations_SpecialtyToInstitutionOfEducationId",
                        column: x => x.SpecialtyToInstitutionOfEducationId,
                        principalTable: "SpecialtyToInstitutionOfEducations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamRequirements",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ExamId = table.Column<string>(nullable: false),
                    SpecialtyToIoEDescriptionId = table.Column<string>(nullable: false),
                    MinimumScore = table.Column<double>(nullable: false),
                    Coefficient = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamRequirements", x => new { x.Id, x.ExamId, x.SpecialtyToIoEDescriptionId });
                    table.ForeignKey(
                        name: "FK_ExamRequirements_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamRequirements_SpecialtyToIoEDescriptions_SpecialtyToIoEDescriptionId",
                        column: x => x.SpecialtyToIoEDescriptionId,
                        principalTable: "SpecialtyToIoEDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamRequirements_ExamId",
                table: "ExamRequirements",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamRequirements_SpecialtyToIoEDescriptionId",
                table: "ExamRequirements",
                column: "SpecialtyToIoEDescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialtyToIoEDescriptions_SpecialtyToInstitutionOfEducationId",
                table: "SpecialtyToIoEDescriptions",
                column: "SpecialtyToInstitutionOfEducationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamRequirements");

            migrationBuilder.DropTable(
                name: "Exams");

            migrationBuilder.DropTable(
                name: "SpecialtyToIoEDescriptions");
        }
    }
}
