using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class renaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpecialityToUniversities");

            migrationBuilder.DropTable(
                name: "Specialities");

            migrationBuilder.CreateTable(
                name: "Specialties",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    DirectionId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Specialties_Directions_DirectionId",
                        column: x => x.DirectionId,
                        principalTable: "Directions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecialtyToUniversities",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SpecialtyId = table.Column<string>(nullable: false),
                    UniversityId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialtyToUniversities", x => new { x.Id, x.UniversityId, x.SpecialtyId });
                    table.ForeignKey(
                        name: "FK_SpecialtyToUniversities_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecialtyToUniversities_Universities_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "Universities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_DirectionId",
                table: "Specialties",
                column: "DirectionId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialtyToUniversities_SpecialtyId",
                table: "SpecialtyToUniversities",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialtyToUniversities_UniversityId",
                table: "SpecialtyToUniversities",
                column: "UniversityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpecialtyToUniversities");

            migrationBuilder.DropTable(
                name: "Specialties");

            migrationBuilder.CreateTable(
                name: "Specialities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DirectionId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Specialities_Directions_DirectionId",
                        column: x => x.DirectionId,
                        principalTable: "Directions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecialityToUniversities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UniversityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SpecialityId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                name: "IX_Specialities_DirectionId",
                table: "Specialities",
                column: "DirectionId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialityToUniversities_SpecialityId",
                table: "SpecialityToUniversities",
                column: "SpecialityId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialityToUniversities_UniversityId",
                table: "SpecialityToUniversities",
                column: "UniversityId");
        }
    }
}
