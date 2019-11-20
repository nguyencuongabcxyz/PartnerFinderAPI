using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class RenameTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestPartners_AspNetUsers_ReceiverId",
                table: "RequestPartners");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestPartners_AspNetUsers_SenderId",
                table: "RequestPartners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestPartners",
                table: "RequestPartners");

            migrationBuilder.RenameTable(
                name: "RequestPartners",
                newName: "PartnerRequests");

            migrationBuilder.RenameIndex(
                name: "IX_RequestPartners_SenderId",
                table: "PartnerRequests",
                newName: "IX_PartnerRequests_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_RequestPartners_ReceiverId",
                table: "PartnerRequests",
                newName: "IX_PartnerRequests_ReceiverId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartnerRequests",
                table: "PartnerRequests",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PartnerRequests_AspNetUsers_ReceiverId",
                table: "PartnerRequests",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PartnerRequests_AspNetUsers_SenderId",
                table: "PartnerRequests",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartnerRequests_AspNetUsers_ReceiverId",
                table: "PartnerRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PartnerRequests_AspNetUsers_SenderId",
                table: "PartnerRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartnerRequests",
                table: "PartnerRequests");

            migrationBuilder.RenameTable(
                name: "PartnerRequests",
                newName: "RequestPartners");

            migrationBuilder.RenameIndex(
                name: "IX_PartnerRequests_SenderId",
                table: "RequestPartners",
                newName: "IX_RequestPartners_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_PartnerRequests_ReceiverId",
                table: "RequestPartners",
                newName: "IX_RequestPartners_ReceiverId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestPartners",
                table: "RequestPartners",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestPartners_AspNetUsers_ReceiverId",
                table: "RequestPartners",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestPartners_AspNetUsers_SenderId",
                table: "RequestPartners",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
