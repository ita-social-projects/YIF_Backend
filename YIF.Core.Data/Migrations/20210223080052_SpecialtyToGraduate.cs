using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class SpecialtyToGraduate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpecialtyToGraduates",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SpecialtyId = table.Column<string>(nullable: false),
                    GraduateId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialtyToGraduates", x => new { x.Id, x.GraduateId, x.SpecialtyId });
                    table.ForeignKey(
                        name: "FK_SpecialtyToGraduates_Graduates_GraduateId",
                        column: x => x.GraduateId,
                        principalTable: "Graduates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecialtyToGraduates_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpecialtyToGraduates_GraduateId",
                table: "SpecialtyToGraduates",
                column: "GraduateId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialtyToGraduates_SpecialtyId",
                table: "SpecialtyToGraduates",
                column: "SpecialtyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpecialtyToGraduates");
        }
    }
}
