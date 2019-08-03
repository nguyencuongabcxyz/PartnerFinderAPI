using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class ModifyBlockRelationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlockedRelations_AspNetUsers_BlockedUserId",
                table: "BlockedRelations");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "BlockedRelations",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "BlockedRelations",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "BlockedUserId",
                table: "BlockedRelations",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BlockedRelations_AspNetUsers_BlockedUserId",
                table: "BlockedRelations",
                column: "BlockedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlockedRelations_AspNetUsers_BlockedUserId",
                table: "BlockedRelations");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "BlockedRelations",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "BlockedRelations",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BlockedUserId",
                table: "BlockedRelations",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_BlockedRelations_AspNetUsers_BlockedUserId",
                table: "BlockedRelations",
                column: "BlockedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
