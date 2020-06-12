using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class addedreferencetocustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "Submissions",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "FtpIdentifier",
                table: "Customers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_CustomerId",
                table: "Submissions",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Customers_CustomerId",
                table: "Submissions",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Customers_CustomerId",
                table: "Submissions");

            migrationBuilder.DropIndex(
                name: "IX_Submissions_CustomerId",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "FtpIdentifier",
                table: "Customers");
        }
    }
}
