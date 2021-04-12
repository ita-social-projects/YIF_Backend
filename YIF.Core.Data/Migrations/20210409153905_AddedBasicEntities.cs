using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class AddedBasicEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InstitutionOfEducations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionOfEducations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schools",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SuperAdmins",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperAdmins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuperAdmins_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Specialties",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.Id);
                });
            
            migrationBuilder.CreateTable(
                name: "InstitutionOfEducationAdmins",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    InstitutionOfEducationId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionOfEducationAdmins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstitutionOfEducationAdmins_InstitutionOfEducations_InstitutionOfEducationId",
                        column: x => x.InstitutionOfEducationId,
                        principalTable: "InstitutionOfEducations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstitutionOfEducationAdmins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lectures",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    InstitutionOfEducationId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lectures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lectures_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lectures_InstitutionOfEducations_InstitutionOfEducationId",
                        column: x => x.InstitutionOfEducationId,
                        principalTable: "InstitutionOfEducations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Graduates",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SchoolId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Graduates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Graduates_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Graduates_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolAdmins",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SchoolId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolAdmins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolAdmins_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SpecialtyToInstitutionOfEducations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SpecialtyId = table.Column<string>(nullable: true),
                    InstitutionOfEducationId = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialtyToInstitutionOfEducations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialtyToInstitutionOfEducations_InstitutionOfEducations_InstitutionOfEducationId",
                        column: x => x.InstitutionOfEducationId,
                        principalTable: "InstitutionOfEducations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecialtyToInstitutionOfEducations_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InstitutionOfEducationModerators",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AdminId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionOfEducationModerators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstitutionOfEducationModerators_InstitutionOfEducationAdmins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "InstitutionOfEducationAdmins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstitutionOfEducationModerators_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SchoolModerators",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SchoolId = table.Column<string>(nullable: true),
                    AdminId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolModerators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolModerators_SchoolAdmins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "SchoolAdmins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolModerators_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolModerators_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Graduates_SchoolId",
                table: "Graduates",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Graduates_UserId",
                table: "Graduates",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionOfEducationAdmins_InstitutionOfEducationId",
                table: "InstitutionOfEducationAdmins",
                column: "InstitutionOfEducationId");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionOfEducationAdmins_UserId",
                table: "InstitutionOfEducationAdmins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionOfEducationModerators_AdminId",
                table: "InstitutionOfEducationModerators",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionOfEducationModerators_UserId",
                table: "InstitutionOfEducationModerators",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_InstitutionOfEducationId",
                table: "Lectures",
                column: "InstitutionOfEducationId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolAdmins_SchoolId",
                table: "SchoolAdmins",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolModerators_AdminId",
                table: "SchoolModerators",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolModerators_SchoolId",
                table: "SchoolModerators",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolModerators_UserId",
                table: "SchoolModerators",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialtyToInstitutionOfEducations_InstitutionOfEducationId",
                table: "SpecialtyToInstitutionOfEducations",
                column: "InstitutionOfEducationId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialtyToInstitutionOfEducations_SpecialtyId",
                table: "SpecialtyToInstitutionOfEducations",
                column: "SpecialtyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstitutionOfEducationModerators");
            
            migrationBuilder.DropTable(
                name: "Lectures");

            migrationBuilder.DropTable(
                name: "SchoolModerators");
            
            migrationBuilder.DropTable(
                name: "SuperAdmins");
            
            migrationBuilder.DropTable(
                name: "InstitutionOfEducationAdmins");

            migrationBuilder.DropTable(
                name: "SchoolAdmins");

            migrationBuilder.DropTable(
                name: "Graduates");

            migrationBuilder.DropTable(
                name: "SpecialtyToInstitutionOfEducations");

            migrationBuilder.DropTable(
                name: "Schools");

            migrationBuilder.DropTable(
                name: "InstitutionOfEducations");

            migrationBuilder.DropTable(
                name: "Specialties");
        }
    }
}
