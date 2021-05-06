using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class ChangedForeignAndPrimaryKeysForLector : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lectures_AspNetUsers_Id",
                table: "Lectures");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Lectures",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_UserId",
                table: "Lectures",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lectures_AspNetUsers_UserId",
                table: "Lectures",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lectures_AspNetUsers_UserId",
                table: "Lectures");

            migrationBuilder.DropIndex(
                name: "IX_Lectures_UserId",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Lectures");

            migrationBuilder.AddForeignKey(
                name: "FK_Lectures_AspNetUsers_Id",
                table: "Lectures",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
