using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class renamedinvitations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvitationEntity_Customers_CustomerEntityId",
                table: "InvitationEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvitationEntity",
                table: "InvitationEntity");

            migrationBuilder.RenameTable(
                name: "InvitationEntity",
                newName: "Invitations");

            migrationBuilder.RenameIndex(
                name: "IX_InvitationEntity_CustomerEntityId",
                table: "Invitations",
                newName: "IX_Invitations_CustomerEntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invitations",
                table: "Invitations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Customers_CustomerEntityId",
                table: "Invitations",
                column: "CustomerEntityId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Customers_CustomerEntityId",
                table: "Invitations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invitations",
                table: "Invitations");

            migrationBuilder.RenameTable(
                name: "Invitations",
                newName: "InvitationEntity");

            migrationBuilder.RenameIndex(
                name: "IX_Invitations_CustomerEntityId",
                table: "InvitationEntity",
                newName: "IX_InvitationEntity_CustomerEntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvitationEntity",
                table: "InvitationEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvitationEntity_Customers_CustomerEntityId",
                table: "InvitationEntity",
                column: "CustomerEntityId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
