using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlowStepEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    From = table.Column<int>(nullable: false),
                    CustomerEntityId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowStepEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlowStepEntity_CustomerEntities_CustomerEntityId",
                        column: x => x.CustomerEntityId,
                        principalTable: "CustomerEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserEntity_CustomerEntities_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "CustomerEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateTable(
                name: "TravelExpenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Stage = table.Column<int>(nullable: false),
                    OwnedByUserEntityId = table.Column<Guid>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsCertified = table.Column<bool>(nullable: false),
                    IsReportedDone = table.Column<bool>(nullable: false),
                    IsAssignedPayment = table.Column<bool>(nullable: false),
                    CustomerEntityId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelExpenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TravelExpenses_CustomerEntities_CustomerEntityId",
                        column: x => x.CustomerEntityId,
                        principalTable: "CustomerEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TravelExpenses_UserEntity_OwnedByUserEntityId",
                        column: x => x.OwnedByUserEntityId,
                        principalTable: "UserEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlowStepEntity_CustomerEntityId",
                table: "FlowStepEntity",
                column: "CustomerEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowStepUserPermissionEntity_UserId",
                table: "FlowStepUserPermissionEntity",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelExpenses_CustomerEntityId",
                table: "TravelExpenses",
                column: "CustomerEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelExpenses_OwnedByUserEntityId",
                table: "TravelExpenses",
                column: "OwnedByUserEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEntity_CustomerId",
                table: "UserEntity",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlowStepUserPermissionEntity");

            migrationBuilder.DropTable(
                name: "TravelExpenses");

            migrationBuilder.DropTable(
                name: "FlowStepEntity");

            migrationBuilder.DropTable(
                name: "UserEntity");

            migrationBuilder.DropTable(
                name: "CustomerEntities");
        }
    }
}
