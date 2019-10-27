using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class removecommenttype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Comments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Comments",
                nullable: false,
                defaultValue: 0);
        }
    }
}
