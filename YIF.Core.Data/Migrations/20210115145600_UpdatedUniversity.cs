using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class UpdatedUniversity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                table: "Universities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Universities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Universities",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndOfCampaign",
                table: "Universities",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<float>(
                name: "Lat",
                table: "Universities",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Lon",
                table: "Universities",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Universities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Site",
                table: "Universities",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartOfCampaign",
                table: "Universities",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abbreviation",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "EndOfCampaign",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "Lat",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "Lon",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "Site",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "StartOfCampaign",
                table: "Universities");
        }
    }
}
