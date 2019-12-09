using Microsoft.EntityFrameworkCore.Migrations;

namespace QienUrenMVC.Migrations
{
    public partial class UserpersonaliaUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPersonalia",
                table: "UserPersonalia");

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "UserPersonalia",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "PersonailiaId",
                table: "UserPersonalia",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPersonalia",
                table: "UserPersonalia",
                column: "PersonailiaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPersonalia",
                table: "UserPersonalia");

            migrationBuilder.DropColumn(
                name: "PersonailiaId",
                table: "UserPersonalia");

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "UserPersonalia",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPersonalia",
                table: "UserPersonalia",
                column: "AccountId");
        }
    }
}
