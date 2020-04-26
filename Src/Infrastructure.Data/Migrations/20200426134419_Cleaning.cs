using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class Cleaning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlowStepEntity_CustomerEntities_CustomerEntityId",
                table: "FlowStepEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_FlowStepUserPermissionEntity_FlowStepEntity_FlowStepId",
                table: "FlowStepUserPermissionEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_FlowStepUserPermissionEntity_UserEntity_UserId",
                table: "FlowStepUserPermissionEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_TravelExpenses_CustomerEntities_CustomerEntityId",
                table: "TravelExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_TravelExpenses_UserEntity_OwnedByUserEntityId",
                table: "TravelExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEntity_CustomerEntities_CustomerId",
                table: "UserEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserEntity",
                table: "UserEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FlowStepUserPermissionEntity",
                table: "FlowStepUserPermissionEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FlowStepEntity",
                table: "FlowStepEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerEntities",
                table: "CustomerEntities");

            migrationBuilder.RenameTable(
                name: "UserEntity",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "FlowStepUserPermissionEntity",
                newName: "FlowStepUserPermissions");

            migrationBuilder.RenameTable(
                name: "FlowStepEntity",
                newName: "FlowSteps");

            migrationBuilder.RenameTable(
                name: "CustomerEntities",
                newName: "Customers");

            migrationBuilder.RenameIndex(
                name: "IX_UserEntity_CustomerId",
                table: "Users",
                newName: "IX_Users_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_FlowStepUserPermissionEntity_UserId",
                table: "FlowStepUserPermissions",
                newName: "IX_FlowStepUserPermissions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FlowStepEntity_CustomerEntityId",
                table: "FlowSteps",
                newName: "IX_FlowSteps_CustomerEntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FlowStepUserPermissions",
                table: "FlowStepUserPermissions",
                columns: new[] { "FlowStepId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_FlowSteps",
                table: "FlowSteps",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FlowSteps_Customers_CustomerEntityId",
                table: "FlowSteps",
                column: "CustomerEntityId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowStepUserPermissions_FlowSteps_FlowStepId",
                table: "FlowStepUserPermissions",
                column: "FlowStepId",
                principalTable: "FlowSteps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowStepUserPermissions_Users_UserId",
                table: "FlowStepUserPermissions",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Customers_CustomerId",
                table: "Users",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlowSteps_Customers_CustomerEntityId",
                table: "FlowSteps");

            migrationBuilder.DropForeignKey(
                name: "FK_FlowStepUserPermissions_FlowSteps_FlowStepId",
                table: "FlowStepUserPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_FlowStepUserPermissions_Users_UserId",
                table: "FlowStepUserPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_TravelExpenses_Customers_CustomerEntityId",
                table: "TravelExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_TravelExpenses_Users_OwnedByUserEntityId",
                table: "TravelExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Customers_CustomerId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FlowStepUserPermissions",
                table: "FlowStepUserPermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FlowSteps",
                table: "FlowSteps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "UserEntity");

            migrationBuilder.RenameTable(
                name: "FlowStepUserPermissions",
                newName: "FlowStepUserPermissionEntity");

            migrationBuilder.RenameTable(
                name: "FlowSteps",
                newName: "FlowStepEntity");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "CustomerEntities");

            migrationBuilder.RenameIndex(
                name: "IX_Users_CustomerId",
                table: "UserEntity",
                newName: "IX_UserEntity_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_FlowStepUserPermissions_UserId",
                table: "FlowStepUserPermissionEntity",
                newName: "IX_FlowStepUserPermissionEntity_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FlowSteps_CustomerEntityId",
                table: "FlowStepEntity",
                newName: "IX_FlowStepEntity_CustomerEntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserEntity",
                table: "UserEntity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FlowStepUserPermissionEntity",
                table: "FlowStepUserPermissionEntity",
                columns: new[] { "FlowStepId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_FlowStepEntity",
                table: "FlowStepEntity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerEntities",
                table: "CustomerEntities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FlowStepEntity_CustomerEntities_CustomerEntityId",
                table: "FlowStepEntity",
                column: "CustomerEntityId",
                principalTable: "CustomerEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowStepUserPermissionEntity_FlowStepEntity_FlowStepId",
                table: "FlowStepUserPermissionEntity",
                column: "FlowStepId",
                principalTable: "FlowStepEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowStepUserPermissionEntity_UserEntity_UserId",
                table: "FlowStepUserPermissionEntity",
                column: "UserId",
                principalTable: "UserEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TravelExpenses_CustomerEntities_CustomerEntityId",
                table: "TravelExpenses",
                column: "CustomerEntityId",
                principalTable: "CustomerEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TravelExpenses_UserEntity_OwnedByUserEntityId",
                table: "TravelExpenses",
                column: "OwnedByUserEntityId",
                principalTable: "UserEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserEntity_CustomerEntities_CustomerId",
                table: "UserEntity",
                column: "CustomerId",
                principalTable: "CustomerEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
