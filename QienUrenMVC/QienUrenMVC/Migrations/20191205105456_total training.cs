using Microsoft.EntityFrameworkCore.Migrations;

namespace QienUrenMVC.Migrations
{
    public partial class totaltraining : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalTraining",
                table: "HoursForm",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalTraining",
                table: "HoursForm");
        }
    }
}
