using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class addedinvitationcustomerrelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Customers_CustomerEntityId",
                table: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_Invitations_CustomerEntityId",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "CustomerEntityId",
                table: "Invitations");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "Invitations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_CustomerId",
                table: "Invitations",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Customers_CustomerId",
                table: "Invitations",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Customers_CustomerId",
                table: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_Invitations_CustomerId",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Invitations");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerEntityId",
                table: "Invitations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_CustomerEntityId",
                table: "Invitations",
                column: "CustomerEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Customers_CustomerEntityId",
                table: "Invitations",
                column: "CustomerEntityId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
