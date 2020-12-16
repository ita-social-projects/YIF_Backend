using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class SpecialityToUniversity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpecialityToUniversities",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SpecialityId = table.Column<string>(nullable: false),
                    UniversityId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialityToUniversities", x => new { x.Id, x.UniversityId, x.SpecialityId });
                    table.ForeignKey(
                        name: "FK_SpecialityToUniversities_Specialities_SpecialityId",
                        column: x => x.SpecialityId,
                        principalTable: "Specialities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecialityToUniversities_Universities_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "Universities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpecialityToUniversities_SpecialityId",
                table: "SpecialityToUniversities",
                column: "SpecialityId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialityToUniversities_UniversityId",
                table: "SpecialityToUniversities",
                column: "UniversityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpecialityToUniversities");
        }
    }
}
