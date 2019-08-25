using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class UpdateQuestionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_LevelTests_LevelTestId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "RightAnwser",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "TestId",
                table: "Questions");

            migrationBuilder.AlterColumn<int>(
                name: "LevelTestId",
                table: "Questions",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRight",
                table: "AnswerOptions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_LevelTests_LevelTestId",
                table: "Questions",
                column: "LevelTestId",
                principalTable: "LevelTests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_LevelTests_LevelTestId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "IsRight",
                table: "AnswerOptions");

            migrationBuilder.AlterColumn<int>(
                name: "LevelTestId",
                table: "Questions",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "RightAnwser",
                table: "Questions",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TestId",
                table: "Questions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_LevelTests_LevelTestId",
                table: "Questions",
                column: "LevelTestId",
                principalTable: "LevelTests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
