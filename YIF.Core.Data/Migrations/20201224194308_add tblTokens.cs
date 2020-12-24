using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class addtblTokens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblTokens",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    RefreshToken = table.Column<string>(maxLength: 100, nullable: false),
                    RefreshTokenExpiryTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblTokens_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblTokens");
        }
    }
}
