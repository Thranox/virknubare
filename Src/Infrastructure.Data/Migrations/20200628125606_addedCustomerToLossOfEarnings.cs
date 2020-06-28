using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class addedCustomerToLossOfEarnings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "LossOfEarningSpecs",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_LossOfEarningSpecs_CustomerId",
                table: "LossOfEarningSpecs",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_LossOfEarningSpecs_Customers_CustomerId",
                table: "LossOfEarningSpecs",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LossOfEarningSpecs_Customers_CustomerId",
                table: "LossOfEarningSpecs");

            migrationBuilder.DropIndex(
                name: "IX_LossOfEarningSpecs_CustomerId",
                table: "LossOfEarningSpecs");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "LossOfEarningSpecs");
        }
    }
}
