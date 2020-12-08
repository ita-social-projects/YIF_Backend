using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class SuperAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuperAdmin_AspNetUsers_Id",
                table: "SuperAdmin");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SuperAdmin",
                table: "SuperAdmin");

            migrationBuilder.RenameTable(
                name: "SuperAdmin",
                newName: "SuperAdmins");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SuperAdmins",
                table: "SuperAdmins",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SuperAdmins_AspNetUsers_Id",
                table: "SuperAdmins",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuperAdmins_AspNetUsers_Id",
                table: "SuperAdmins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SuperAdmins",
                table: "SuperAdmins");

            migrationBuilder.RenameTable(
                name: "SuperAdmins",
                newName: "SuperAdmin");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SuperAdmin",
                table: "SuperAdmin",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SuperAdmin_AspNetUsers_Id",
                table: "SuperAdmin",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
