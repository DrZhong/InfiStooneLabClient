using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guoxu.LabManager.Migrations
{
    public partial class _021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MasterUserId",
                table: "Warehouse",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LockedOrderId",
                table: "ReagentStock",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ClientConfirm",
                table: "Reagent",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DoubleConfirm",
                table: "Reagent",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "OutOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OutOrderStatus = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    WarehouseName = table.Column<int>(type: "int", nullable: false),
                    OutOrderType = table.Column<int>(type: "int", nullable: false),
                    ApplyUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutOrder_AbpUsers_ApplyUserId",
                        column: x => x.ApplyUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReagentStockAudit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReagentStockId = table.Column<int>(type: "int", nullable: false),
                    ReagentStockAuditType = table.Column<int>(type: "int", nullable: false),
                    AuditUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditUserId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReagentStockAudit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReagentStockAudit_AbpUsers_AuditUserId",
                        column: x => x.AuditUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReagentStockAudit_ReagentStock_ReagentStockId",
                        column: x => x.ReagentStockId,
                        principalTable: "ReagentStock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutOrderMasterItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OutOrderId = table.Column<int>(type: "int", nullable: false),
                    IsAudit = table.Column<bool>(type: "bit", nullable: false),
                    ReagentStockId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutOrderMasterItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutOrderMasterItem_OutOrder_OutOrderId",
                        column: x => x.OutOrderId,
                        principalTable: "OutOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutOrderMasterItem_ReagentStock_ReagentStockId",
                        column: x => x.ReagentStockId,
                        principalTable: "ReagentStock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Warehouse_MasterUserId",
                table: "Warehouse",
                column: "MasterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OutOrder_ApplyUserId",
                table: "OutOrder",
                column: "ApplyUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OutOrderMasterItem_OutOrderId",
                table: "OutOrderMasterItem",
                column: "OutOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OutOrderMasterItem_ReagentStockId",
                table: "OutOrderMasterItem",
                column: "ReagentStockId");

            migrationBuilder.CreateIndex(
                name: "IX_ReagentStockAudit_AuditUserId",
                table: "ReagentStockAudit",
                column: "AuditUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReagentStockAudit_ReagentStockId",
                table: "ReagentStockAudit",
                column: "ReagentStockId");

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouse_AbpUsers_MasterUserId",
                table: "Warehouse",
                column: "MasterUserId",
                principalTable: "AbpUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Warehouse_AbpUsers_MasterUserId",
                table: "Warehouse");

            migrationBuilder.DropTable(
                name: "OutOrderMasterItem");

            migrationBuilder.DropTable(
                name: "ReagentStockAudit");

            migrationBuilder.DropTable(
                name: "OutOrder");

            migrationBuilder.DropIndex(
                name: "IX_Warehouse_MasterUserId",
                table: "Warehouse");

            migrationBuilder.DropColumn(
                name: "MasterUserId",
                table: "Warehouse");

            migrationBuilder.DropColumn(
                name: "LockedOrderId",
                table: "ReagentStock");

            migrationBuilder.DropColumn(
                name: "ClientConfirm",
                table: "Reagent");

            migrationBuilder.DropColumn(
                name: "DoubleConfirm",
                table: "Reagent");
        }
    }
}
