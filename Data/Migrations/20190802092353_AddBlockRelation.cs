using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddBlockRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Partnership_AspNetUsers_OwnerId",
                table: "Partnership");

            migrationBuilder.DropForeignKey(
                name: "FK_Partnership_AspNetUsers_PartnerId",
                table: "Partnership");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Partnership",
                table: "Partnership");

            migrationBuilder.RenameTable(
                name: "Partnership",
                newName: "Partnerships");

            migrationBuilder.RenameIndex(
                name: "IX_Partnership_PartnerId",
                table: "Partnerships",
                newName: "IX_Partnerships_PartnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Partnership_OwnerId",
                table: "Partnerships",
                newName: "IX_Partnerships_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Partnerships",
                table: "Partnerships",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BlockedRelations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OwnerId = table.Column<string>(nullable: true),
                    BlockedUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockedRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlockedRelations_AspNetUsers_BlockedUserId",
                        column: x => x.BlockedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlockedRelations_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlockedRelations_BlockedUserId",
                table: "BlockedRelations",
                column: "BlockedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlockedRelations_OwnerId",
                table: "BlockedRelations",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Partnerships_AspNetUsers_OwnerId",
                table: "Partnerships",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Partnerships_AspNetUsers_PartnerId",
                table: "Partnerships",
                column: "PartnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Partnerships_AspNetUsers_OwnerId",
                table: "Partnerships");

            migrationBuilder.DropForeignKey(
                name: "FK_Partnerships_AspNetUsers_PartnerId",
                table: "Partnerships");

            migrationBuilder.DropTable(
                name: "BlockedRelations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Partnerships",
                table: "Partnerships");

            migrationBuilder.RenameTable(
                name: "Partnerships",
                newName: "Partnership");

            migrationBuilder.RenameIndex(
                name: "IX_Partnerships_PartnerId",
                table: "Partnership",
                newName: "IX_Partnership_PartnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Partnerships_OwnerId",
                table: "Partnership",
                newName: "IX_Partnership_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Partnership",
                table: "Partnership",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Partnership_AspNetUsers_OwnerId",
                table: "Partnership",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Partnership_AspNetUsers_PartnerId",
                table: "Partnership",
                column: "PartnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
