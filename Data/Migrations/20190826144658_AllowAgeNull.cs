using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AllowAgeNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Age",
                table: "UserInformations",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Age",
                table: "UserInformations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
