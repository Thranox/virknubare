using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class renamedfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "TravelExpenses");

            migrationBuilder.AddColumn<bool>(
                name: "IsCertified",
                table: "TravelExpenses",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCertified",
                table: "TravelExpenses");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "TravelExpenses",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
