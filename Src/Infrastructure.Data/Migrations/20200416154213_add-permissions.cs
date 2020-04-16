using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class addpermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CustomerEntityId",
                table: "TravelExpenses",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FlowStepUserPermissionEntity",
                columns: table => new
                {
                    FlowStepId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowStepUserPermissionEntity", x => new { x.FlowStepId, x.UserId });
                    table.ForeignKey(
                        name: "FK_FlowStepUserPermissionEntity_FlowStepEntity_FlowStepId",
                        column: x => x.FlowStepId,
                        principalTable: "FlowStepEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlowStepUserPermissionEntity_UserEntity_UserId",
                        column: x => x.UserId,
                        principalTable: "UserEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TravelExpenses_CustomerEntityId",
                table: "TravelExpenses",
                column: "CustomerEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowStepUserPermissionEntity_UserId",
                table: "FlowStepUserPermissionEntity",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TravelExpenses_CustomerEntities_CustomerEntityId",
                table: "TravelExpenses",
                column: "CustomerEntityId",
                principalTable: "CustomerEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TravelExpenses_CustomerEntities_CustomerEntityId",
                table: "TravelExpenses");

            migrationBuilder.DropTable(
                name: "FlowStepUserPermissionEntity");

            migrationBuilder.DropIndex(
                name: "IX_TravelExpenses_CustomerEntityId",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "CustomerEntityId",
                table: "TravelExpenses");
        }
    }
}
