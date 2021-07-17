using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class AddedBufferForIoE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Specialties",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Lectors",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "IoEBuffers",
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
                    IoEStatus = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IoEBuffers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IoEBuffers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Specialties");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Lectors");
        }
    }
}
