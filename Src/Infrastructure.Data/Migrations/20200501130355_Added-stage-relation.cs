using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class Addedstagerelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAssignedPayment",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "IsCertified",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "IsReportedDone",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "Stage",
                table: "TravelExpenses");

            migrationBuilder.AddColumn<Guid>(
                name: "StageId",
                table: "TravelExpenses",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TravelExpenses_StageId",
                table: "TravelExpenses",
                column: "StageId");

            migrationBuilder.AddForeignKey(
                name: "FK_TravelExpenses_Stages_StageId",
                table: "TravelExpenses",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TravelExpenses_Stages_StageId",
                table: "TravelExpenses");

            migrationBuilder.DropIndex(
                name: "IX_TravelExpenses_StageId",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "StageId",
                table: "TravelExpenses");

            migrationBuilder.AddColumn<bool>(
                name: "IsAssignedPayment",
                table: "TravelExpenses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCertified",
                table: "TravelExpenses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReportedDone",
                table: "TravelExpenses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Stage",
                table: "TravelExpenses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
