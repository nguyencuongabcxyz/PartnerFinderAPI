using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class UpdateInfoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EnglishSkill",
                table: "UserInformations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Expectation",
                table: "UserInformations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LearningSkill",
                table: "UserInformations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnglishSkill",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "Expectation",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "LearningSkill",
                table: "UserInformations");
        }
    }
}
