using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class addusercolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFinalized",
                table: "TravelExpenses");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UserEntity",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "UserEntity",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "UserEntity");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "UserEntity");

            migrationBuilder.AddColumn<bool>(
                name: "IsFinalized",
                table: "TravelExpenses",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
