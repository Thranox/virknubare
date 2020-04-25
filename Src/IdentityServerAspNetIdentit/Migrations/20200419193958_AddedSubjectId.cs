using Microsoft.EntityFrameworkCore.Migrations;

namespace IDP.Migrations
{
    public partial class AddedSubjectId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubjectId",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "AspNetUsers");
        }
    }
}
