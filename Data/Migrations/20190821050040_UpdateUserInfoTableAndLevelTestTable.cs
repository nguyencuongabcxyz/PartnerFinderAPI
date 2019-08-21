using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class UpdateUserInfoTableAndLevelTestTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LevelTests_AspNetUsers_UserId",
                table: "LevelTests");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "LevelTests",
                newName: "AdminId");

            migrationBuilder.RenameIndex(
                name: "IX_LevelTests_UserId",
                table: "LevelTests",
                newName: "IX_LevelTests_AdminId");

            migrationBuilder.AlterColumn<int>(
                name: "Level",
                table: "UserInformations",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_LevelTests_AspNetUsers_AdminId",
                table: "LevelTests",
                column: "AdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LevelTests_AspNetUsers_AdminId",
                table: "LevelTests");

            migrationBuilder.RenameColumn(
                name: "AdminId",
                table: "LevelTests",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_LevelTests_AdminId",
                table: "LevelTests",
                newName: "IX_LevelTests_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "Level",
                table: "UserInformations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LevelTests_AspNetUsers_UserId",
                table: "LevelTests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
