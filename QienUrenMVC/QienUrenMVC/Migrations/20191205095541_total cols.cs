using Microsoft.EntityFrameworkCore.Migrations;

namespace QienUrenMVC.Migrations
{
    public partial class totalcols : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalLeave",
                table: "HoursForm",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalOther",
                table: "HoursForm",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalOver",
                table: "HoursForm",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalSick",
                table: "HoursForm",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalLeave",
                table: "HoursForm");

            migrationBuilder.DropColumn(
                name: "TotalOther",
                table: "HoursForm");

            migrationBuilder.DropColumn(
                name: "TotalOver",
                table: "HoursForm");

            migrationBuilder.DropColumn(
                name: "TotalSick",
                table: "HoursForm");
        }
    }
}
