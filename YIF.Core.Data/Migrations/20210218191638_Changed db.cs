using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class Changeddb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SpecialtyInUniversityDescriptionId",
                table: "SpecialtyToUniversities",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EducationForms",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationForms", x => x.Id);
                });

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
                name: "PaymentForms",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentForms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpecialtyInUniversityDescriptions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    EducationalProgramLink = table.Column<string>(nullable: true),
                    EducationFormId = table.Column<string>(nullable: true),
                    PaymentFormId = table.Column<string>(nullable: true),
                    ExamRequirementId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialtyInUniversityDescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialtyInUniversityDescriptions_EducationForms_EducationFormId",
                        column: x => x.EducationFormId,
                        principalTable: "EducationForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SpecialtyInUniversityDescriptions_PaymentForms_PaymentFormId",
                        column: x => x.PaymentFormId,
                        principalTable: "PaymentForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExamToSpecialtyToUniversities",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ExamId = table.Column<string>(nullable: true),
                    SpecialtyInUniversityDescriptionId = table.Column<string>(nullable: true),
                    MinimumScore = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamToSpecialtyToUniversities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamToSpecialtyToUniversities_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExamToSpecialtyToUniversities_SpecialtyInUniversityDescriptions_SpecialtyInUniversityDescriptionId",
                        column: x => x.SpecialtyInUniversityDescriptionId,
                        principalTable: "SpecialtyInUniversityDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpecialtyToUniversities_SpecialtyInUniversityDescriptionId",
                table: "SpecialtyToUniversities",
                column: "SpecialtyInUniversityDescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamToSpecialtyToUniversities_ExamId",
                table: "ExamToSpecialtyToUniversities",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamToSpecialtyToUniversities_SpecialtyInUniversityDescriptionId",
                table: "ExamToSpecialtyToUniversities",
                column: "SpecialtyInUniversityDescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialtyInUniversityDescriptions_EducationFormId",
                table: "SpecialtyInUniversityDescriptions",
                column: "EducationFormId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialtyInUniversityDescriptions_ExamRequirementId",
                table: "SpecialtyInUniversityDescriptions",
                column: "ExamRequirementId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialtyInUniversityDescriptions_PaymentFormId",
                table: "SpecialtyInUniversityDescriptions",
                column: "PaymentFormId",
                unique: true,
                filter: "[PaymentFormId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialtyToUniversities_SpecialtyInUniversityDescriptions_SpecialtyInUniversityDescriptionId",
                table: "SpecialtyToUniversities",
                column: "SpecialtyInUniversityDescriptionId",
                principalTable: "SpecialtyInUniversityDescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialtyInUniversityDescriptions_ExamToSpecialtyToUniversities_ExamRequirementId",
                table: "SpecialtyInUniversityDescriptions",
                column: "ExamRequirementId",
                principalTable: "ExamToSpecialtyToUniversities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecialtyToUniversities_SpecialtyInUniversityDescriptions_SpecialtyInUniversityDescriptionId",
                table: "SpecialtyToUniversities");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamToSpecialtyToUniversities_Exams_ExamId",
                table: "ExamToSpecialtyToUniversities");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamToSpecialtyToUniversities_SpecialtyInUniversityDescriptions_SpecialtyInUniversityDescriptionId",
                table: "ExamToSpecialtyToUniversities");

            migrationBuilder.DropTable(
                name: "Exams");

            migrationBuilder.DropTable(
                name: "SpecialtyInUniversityDescriptions");

            migrationBuilder.DropTable(
                name: "EducationForms");

            migrationBuilder.DropTable(
                name: "ExamToSpecialtyToUniversities");

            migrationBuilder.DropTable(
                name: "PaymentForms");

            migrationBuilder.DropIndex(
                name: "IX_SpecialtyToUniversities_SpecialtyInUniversityDescriptionId",
                table: "SpecialtyToUniversities");

            migrationBuilder.DropColumn(
                name: "SpecialtyInUniversityDescriptionId",
                table: "SpecialtyToUniversities");
        }
    }
}
