using Microsoft.EntityFrameworkCore.Migrations;

namespace QienUrenMVC.Migrations
{
    public partial class UserHasChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsChanged",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HoursPerDay_FormId",
                table: "HoursPerDay",
                column: "FormId");

            migrationBuilder.AddForeignKey(
                name: "FK_HoursPerDay_HoursForm_FormId",
                table: "HoursPerDay",
                column: "FormId",
                principalTable: "HoursForm",
                principalColumn: "FormId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoursPerDay_HoursForm_FormId",
                table: "HoursPerDay");

            migrationBuilder.DropIndex(
                name: "IX_HoursPerDay_FormId",
                table: "HoursPerDay");

            migrationBuilder.DropColumn(
                name: "IsChanged",
                table: "AspNetUsers");
        }
    }
}
