using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace YIF.Core.Data.Migrations
{
    public partial class AddedTermsOfCampaignForIoE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "StartOfCampaign",
                table: "InstitutionOfEducations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndOfCampaign",
                table: "InstitutionOfEducations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartOfCampaign",
                table: "InstitutionOfEducations");

            migrationBuilder.DropColumn(
                name: "EndOfCampaign",
                table: "InstitutionOfEducations");
        }
    }
}
