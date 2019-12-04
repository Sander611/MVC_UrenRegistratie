using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QienUrenMVC.Migrations
{
    public partial class UserPersonalia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserPersonalia",
                columns: table => new
                {
                    AccountId = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    ZIP = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    IBAN = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    ProfileImage = table.Column<string>(nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false),
                    IsTrainee = table.Column<bool>(nullable: false),
                    IsQienEmployee = table.Column<bool>(nullable: false),
                    IsSeniorDeveloper = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsChanged = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPersonalia", x => x.AccountId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPersonalia");
        }
    }
}
