using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class AddedDirections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Directions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Directions", x => x.Id);
                });

            migrationBuilder.AddColumn<string>(
                name: "DirectionId",
                table: "Specialties",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Specialties_Directions_DirectionId",
                table: "Specialties",
                column: "DirectionId",
                principalTable: "Directions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.CreateTable(
                name: "DirectionsToInstitutionOfEducations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    DirectionId = table.Column<string>(nullable: false),
                    InstitutionOfEducationId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectionsToInstitutionOfEducations", x => new { x.Id, x.InstitutionOfEducationId, x.DirectionId });
                    table.ForeignKey(
                        name: "FK_DirectionsToInstitutionOfEducations_Directions_DirectionId",
                        column: x => x.DirectionId,
                        principalTable: "Directions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DirectionsToInstitutionOfEducations_InstitutionOfEducations_InstitutionOfEducationId",
                        column: x => x.InstitutionOfEducationId,
                        principalTable: "InstitutionOfEducations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DirectionsToInstitutionOfEducations_DirectionId",
                table: "DirectionsToInstitutionOfEducations",
                column: "DirectionId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectionsToInstitutionOfEducations_InstitutionOfEducationId",
                table: "DirectionsToInstitutionOfEducations",
                column: "InstitutionOfEducationId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_DirectionId",
                table: "Specialties",
                column: "DirectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DirectionsToInstitutionOfEducations");

            migrationBuilder.DropColumn(
                name: "DirectionId",
                table: "Specialties");

            migrationBuilder.DropForeignKey(
                name: "FK_Specialties_Directions_DirectionId",
                table: "Specialties");

            migrationBuilder.DropTable(
                name: "Directions");
        }
    }
}
