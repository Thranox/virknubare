using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class Initial0710 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerUserPermissionEntity_Customers_CustomerId",
                table: "CustomerUserPermissionEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerUserPermissionEntity_Users_UserId",
                table: "CustomerUserPermissionEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_TravelExpenses_Customers_CustomerEntityId",
                table: "TravelExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_TravelExpenses_Users_OwnedByUserEntityId",
                table: "TravelExpenses");

            migrationBuilder.DropIndex(
                name: "IX_TravelExpenses_CustomerEntityId",
                table: "TravelExpenses");

            migrationBuilder.DropIndex(
                name: "IX_TravelExpenses_OwnedByUserEntityId",
                table: "TravelExpenses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerUserPermissionEntity",
                table: "CustomerUserPermissionEntity");

            migrationBuilder.DropColumn(
                name: "CustomerEntityId",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "OwnedByUserEntityId",
                table: "TravelExpenses");

            migrationBuilder.RenameTable(
                name: "CustomerUserPermissionEntity",
                newName: "CustomerUserPermissions");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerUserPermissionEntity_UserId",
                table: "CustomerUserPermissions",
                newName: "IX_CustomerUserPermissions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerUserPermissionEntity_CustomerId",
                table: "CustomerUserPermissions",
                newName: "IX_CustomerUserPermissions_CustomerId");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "TravelExpenses",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OwnedByUserId",
                table: "TravelExpenses",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerUserPermissions",
                table: "CustomerUserPermissions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Stages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Value = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TravelExpenses_CustomerId",
                table: "TravelExpenses",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelExpenses_OwnedByUserId",
                table: "TravelExpenses",
                column: "OwnedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerUserPermissions_Customers_CustomerId",
                table: "CustomerUserPermissions",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerUserPermissions_Users_UserId",
                table: "CustomerUserPermissions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TravelExpenses_Customers_CustomerId",
                table: "TravelExpenses",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TravelExpenses_Users_OwnedByUserId",
                table: "TravelExpenses",
                column: "OwnedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerUserPermissions_Customers_CustomerId",
                table: "CustomerUserPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerUserPermissions_Users_UserId",
                table: "CustomerUserPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_TravelExpenses_Customers_CustomerId",
                table: "TravelExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_TravelExpenses_Users_OwnedByUserId",
                table: "TravelExpenses");

            migrationBuilder.DropTable(
                name: "Stages");

            migrationBuilder.DropIndex(
                name: "IX_TravelExpenses_CustomerId",
                table: "TravelExpenses");

            migrationBuilder.DropIndex(
                name: "IX_TravelExpenses_OwnedByUserId",
                table: "TravelExpenses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerUserPermissions",
                table: "CustomerUserPermissions");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "TravelExpenses");

            migrationBuilder.DropColumn(
                name: "OwnedByUserId",
                table: "TravelExpenses");

            migrationBuilder.RenameTable(
                name: "CustomerUserPermissions",
                newName: "CustomerUserPermissionEntity");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerUserPermissions_UserId",
                table: "CustomerUserPermissionEntity",
                newName: "IX_CustomerUserPermissionEntity_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerUserPermissions_CustomerId",
                table: "CustomerUserPermissionEntity",
                newName: "IX_CustomerUserPermissionEntity_CustomerId");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerEntityId",
                table: "TravelExpenses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OwnedByUserEntityId",
                table: "TravelExpenses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerUserPermissionEntity",
                table: "CustomerUserPermissionEntity",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TravelExpenses_CustomerEntityId",
                table: "TravelExpenses",
                column: "CustomerEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelExpenses_OwnedByUserEntityId",
                table: "TravelExpenses",
                column: "OwnedByUserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerUserPermissionEntity_Customers_CustomerId",
                table: "CustomerUserPermissionEntity",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerUserPermissionEntity_Users_UserId",
                table: "CustomerUserPermissionEntity",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TravelExpenses_Customers_CustomerEntityId",
                table: "TravelExpenses",
                column: "CustomerEntityId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TravelExpenses_Users_OwnedByUserEntityId",
                table: "TravelExpenses",
                column: "OwnedByUserEntityId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
