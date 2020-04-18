using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class addusercolumnsii : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OwnedByUserEntityId",
                table: "TravelExpenses",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TravelExpenses_OwnedByUserEntityId",
                table: "TravelExpenses",
                column: "OwnedByUserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_TravelExpenses_UserEntity_OwnedByUserEntityId",
                table: "TravelExpenses",
                column: "OwnedByUserEntityId",
                principalTable: "UserEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TravelExpenses_UserEntity_OwnedByUserEntityId",
                table: "TravelExpenses");

            migrationBuilder.DropIndex(
                name: "IX_TravelExpenses_OwnedByUserEntityId",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "OwnedByUserEntityId",
                table: "TravelExpenses");
        }
    }
}
