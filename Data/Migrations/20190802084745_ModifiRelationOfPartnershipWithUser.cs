using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class ModifiRelationOfPartnershipWithUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PartnerId",
                table: "Partnership",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Partnership_PartnerId",
                table: "Partnership",
                column: "PartnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Partnership_AspNetUsers_PartnerId",
                table: "Partnership",
                column: "PartnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Partnership_AspNetUsers_PartnerId",
                table: "Partnership");

            migrationBuilder.DropIndex(
                name: "IX_Partnership_PartnerId",
                table: "Partnership");

            migrationBuilder.AlterColumn<string>(
                name: "PartnerId",
                table: "Partnership",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
