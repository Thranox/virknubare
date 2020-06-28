using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class addedLossOfEarningsToTravelExpense : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LossOfEarnings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    NumberOfHours = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    TravelExpenseId = table.Column<Guid>(nullable: true),
                    LossOfEarningSpecId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LossOfEarnings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LossOfEarnings_LossOfEarningSpecs_LossOfEarningSpecId",
                        column: x => x.LossOfEarningSpecId,
                        principalTable: "LossOfEarningSpecs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LossOfEarnings_TravelExpenses_TravelExpenseId",
                        column: x => x.TravelExpenseId,
                        principalTable: "TravelExpenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LossOfEarnings_LossOfEarningSpecId",
                table: "LossOfEarnings",
                column: "LossOfEarningSpecId");

            migrationBuilder.CreateIndex(
                name: "IX_LossOfEarnings_TravelExpenseId",
                table: "LossOfEarnings",
                column: "TravelExpenseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LossOfEarnings");
        }
    }
}
