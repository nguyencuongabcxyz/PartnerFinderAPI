using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class ReaddUpVoteField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UpVote",
                table: "Posts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Like",
                table: "Comments",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpVote",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Like",
                table: "Comments");
        }
    }
}
