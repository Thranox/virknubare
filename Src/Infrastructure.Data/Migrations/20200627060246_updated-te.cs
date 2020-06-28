using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once IdentifierTypo
    // ReSharper disable once UnusedMember.Global
    public partial class updatedte : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TravelExpenses_Customers_CustomerId",
                table: "TravelExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_TravelExpenses_Users_OwnedByUserId",
                table: "TravelExpenses");

            migrationBuilder.DropIndex(
                name: "IX_TravelExpenses_CustomerId",
                table: "TravelExpenses");

            migrationBuilder.DropIndex(
                name: "IX_TravelExpenses_OwnedByUserId",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "ArrivalDateTime",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "CommitteeId",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "DepartureDateTime",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "Expenses",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "IsAbsenceAllowance",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "IsEducationalPurpose",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "OwnedByUserId",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "Purpose",
                table: "TravelExpenses");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerEntityId",
                table: "TravelExpenses",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserEntityId",
                table: "TravelExpenses",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TravelExpenses_CustomerEntityId",
                table: "TravelExpenses",
                column: "CustomerEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelExpenses_UserEntityId",
                table: "TravelExpenses",
                column: "UserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_TravelExpenses_Customers_CustomerEntityId",
                table: "TravelExpenses",
                column: "CustomerEntityId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TravelExpenses_Users_UserEntityId",
                table: "TravelExpenses",
                column: "UserEntityId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TravelExpenses_Customers_CustomerEntityId",
                table: "TravelExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_TravelExpenses_Users_UserEntityId",
                table: "TravelExpenses");

            migrationBuilder.DropIndex(
                name: "IX_TravelExpenses_CustomerEntityId",
                table: "TravelExpenses");

            migrationBuilder.DropIndex(
                name: "IX_TravelExpenses_UserEntityId",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "CustomerEntityId",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "UserEntityId",
                table: "TravelExpenses");

            migrationBuilder.AddColumn<DateTime>(
                name: "ArrivalDateTime",
                table: "TravelExpenses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CommitteeId",
                table: "TravelExpenses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "TravelExpenses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DepartureDateTime",
                table: "TravelExpenses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "Expenses",
                table: "TravelExpenses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAbsenceAllowance",
                table: "TravelExpenses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEducationalPurpose",
                table: "TravelExpenses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "OwnedByUserId",
                table: "TravelExpenses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Purpose",
                table: "TravelExpenses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TravelExpenses_CustomerId",
                table: "TravelExpenses",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelExpenses_OwnedByUserId",
                table: "TravelExpenses",
                column: "OwnedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TravelExpenses_Customers_CustomerId",
                table: "TravelExpenses",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TravelExpenses_Users_OwnedByUserId",
                table: "TravelExpenses",
                column: "OwnedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
