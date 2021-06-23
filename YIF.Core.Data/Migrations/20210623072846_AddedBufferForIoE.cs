using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class AddedBufferForIoE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InstitutionOfEducationBuffers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Abbreviation = table.Column<string>(nullable: true),
                    Site = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true),
                    Lat = table.Column<float>(nullable: false),
                    Lon = table.Column<float>(nullable: false),
                    IsBanned = table.Column<bool>(nullable: false),
                    InstitutionOfEducationType = table.Column<int>(nullable: false),
                    StartOfCampaign = table.Column<DateTime>(nullable: false),
                    EndOfCampaign = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    InstitutionOfEducationStatus = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    InstitutionOfEducationId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionOfEducationBuffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstitutionOfEducationBuffers_InstitutionOfEducations_InstitutionOfEducationId",
                        column: x => x.InstitutionOfEducationId,
                        principalTable: "InstitutionOfEducations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionOfEducationBuffers_InstitutionOfEducationId",
                table: "InstitutionOfEducationBuffers",
                column: "InstitutionOfEducationId",
                unique: true,
                filter: "[InstitutionOfEducationId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstitutionOfEducationBuffers");
        }
    }
}
