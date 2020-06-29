using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class addedPlaces : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArrivalPlace",
                table: "TravelExpenses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeparturePlace",
                table: "TravelExpenses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArrivalPlace",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "DeparturePlace",
                table: "TravelExpenses");
        }
    }
}
