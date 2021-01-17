using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class AddedUniversityGraduates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UniversitiesToGraduates",
                columns: table => new
                {
                    GraduateId = table.Column<string>(nullable: false),
                    UniversityId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniversitiesToGraduates", x => new { x.UniversityId, x.GraduateId });
                    table.ForeignKey(
                        name: "FK_UniversitiesToGraduates_Graduates_GraduateId",
                        column: x => x.GraduateId,
                        principalTable: "Graduates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UniversitiesToGraduates_Universities_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "Universities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UniversitiesToGraduates_GraduateId",
                table: "UniversitiesToGraduates",
                column: "GraduateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UniversitiesToGraduates");
        }
    }
}
