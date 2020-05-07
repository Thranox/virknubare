using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class Initial0832 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlowSteps_Customers_CustomerEntityId",
                table: "FlowSteps");

            migrationBuilder.DropIndex(
                name: "IX_FlowSteps_CustomerEntityId",
                table: "FlowSteps");

            migrationBuilder.DropColumn(
                name: "CustomerEntityId",
                table: "FlowSteps");

            migrationBuilder.DropColumn(
                name: "From",
                table: "FlowSteps");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "FlowSteps",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FromId",
                table: "FlowSteps",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlowSteps_CustomerId",
                table: "FlowSteps",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowSteps_FromId",
                table: "FlowSteps",
                column: "FromId");

            migrationBuilder.AddForeignKey(
                name: "FK_FlowSteps_Customers_CustomerId",
                table: "FlowSteps",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowSteps_Stages_FromId",
                table: "FlowSteps",
                column: "FromId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlowSteps_Customers_CustomerId",
                table: "FlowSteps");

            migrationBuilder.DropForeignKey(
                name: "FK_FlowSteps_Stages_FromId",
                table: "FlowSteps");

            migrationBuilder.DropIndex(
                name: "IX_FlowSteps_CustomerId",
                table: "FlowSteps");

            migrationBuilder.DropIndex(
                name: "IX_FlowSteps_FromId",
                table: "FlowSteps");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "FlowSteps");

            migrationBuilder.DropColumn(
                name: "FromId",
                table: "FlowSteps");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerEntityId",
                table: "FlowSteps",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "From",
                table: "FlowSteps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FlowSteps_CustomerEntityId",
                table: "FlowSteps",
                column: "CustomerEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_FlowSteps_Customers_CustomerEntityId",
                table: "FlowSteps",
                column: "CustomerEntityId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
