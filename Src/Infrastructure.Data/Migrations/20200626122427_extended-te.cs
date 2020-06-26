using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class extendedte : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ArrivalDateTime",
                table: "TravelExpenses",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CommitteeId",
                table: "TravelExpenses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DailyAllowanceAmount",
                table: "TravelExpenses",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DepartureDateTime",
                table: "TravelExpenses",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DestinationPlace",
                table: "TravelExpenses",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Expenses",
                table: "TravelExpenses",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "FoodAllowances",
                table: "TravelExpenses",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAbsenceAllowance",
                table: "TravelExpenses",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEducationalPurpose",
                table: "TravelExpenses",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Purpose",
                table: "TravelExpenses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransportSpecification",
                table: "TravelExpenses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArrivalDateTime",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "CommitteeId",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "DailyAllowanceAmount",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "DepartureDateTime",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "DestinationPlace",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "Expenses",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "FoodAllowances",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "IsAbsenceAllowance",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "IsEducationalPurpose",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "Purpose",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "TransportSpecification",
                table: "TravelExpenses");
        }
    }
}
