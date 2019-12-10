using Microsoft.EntityFrameworkCore.Migrations;

namespace QienUrenMVC.Migrations
{
    public partial class MobilephoneUserPersonalia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MobilePhone",
                table: "UserPersonalia",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MobilePhone",
                table: "UserPersonalia");
        }
    }
}
