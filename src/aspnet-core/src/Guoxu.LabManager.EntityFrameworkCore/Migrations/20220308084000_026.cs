using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guoxu.LabManager.Migrations
{
    public partial class _026 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OutOrderMasterItem_ReagentStock_ReagentStockId",
                table: "OutOrderMasterItem");

            migrationBuilder.RenameColumn(
                name: "IsAudit",
                table: "OutOrderMasterItem",
                newName: "DoubleConfirmed");

            migrationBuilder.AlterColumn<int>(
                name: "ReagentStockId",
                table: "OutOrderMasterItem",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "ClientConfirm",
                table: "OutOrderMasterItem",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ClientConfirmed",
                table: "OutOrderMasterItem",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DoubleConfirm",
                table: "OutOrderMasterItem",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NormalReagentStockId",
                table: "OutOrderMasterItem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockoutAccount",
                table: "OutOrderMasterItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OutOrderMasterItemAudit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OutOrderMasterItemId = table.Column<int>(type: "int", nullable: false),
                    ReagentStockAuditType = table.Column<int>(type: "int", nullable: false),
                    AuditUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditUserId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutOrderMasterItemAudit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutOrderMasterItemAudit_AbpUsers_AuditUserId",
                        column: x => x.AuditUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutOrderMasterItemAudit_OutOrderMasterItem_OutOrderMasterItemId",
                        column: x => x.OutOrderMasterItemId,
                        principalTable: "OutOrderMasterItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutOrderMasterItem_NormalReagentStockId",
                table: "OutOrderMasterItem",
                column: "NormalReagentStockId");

            migrationBuilder.CreateIndex(
                name: "IX_OutOrderMasterItemAudit_AuditUserId",
                table: "OutOrderMasterItemAudit",
                column: "AuditUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OutOrderMasterItemAudit_OutOrderMasterItemId",
                table: "OutOrderMasterItemAudit",
                column: "OutOrderMasterItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_OutOrderMasterItem_NormalReagentStock_NormalReagentStockId",
                table: "OutOrderMasterItem",
                column: "NormalReagentStockId",
                principalTable: "NormalReagentStock",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OutOrderMasterItem_ReagentStock_ReagentStockId",
                table: "OutOrderMasterItem",
                column: "ReagentStockId",
                principalTable: "ReagentStock",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OutOrderMasterItem_NormalReagentStock_NormalReagentStockId",
                table: "OutOrderMasterItem");

            migrationBuilder.DropForeignKey(
                name: "FK_OutOrderMasterItem_ReagentStock_ReagentStockId",
                table: "OutOrderMasterItem");

            migrationBuilder.DropTable(
                name: "OutOrderMasterItemAudit");

            migrationBuilder.DropIndex(
                name: "IX_OutOrderMasterItem_NormalReagentStockId",
                table: "OutOrderMasterItem");

            migrationBuilder.DropColumn(
                name: "ClientConfirm",
                table: "OutOrderMasterItem");

            migrationBuilder.DropColumn(
                name: "ClientConfirmed",
                table: "OutOrderMasterItem");

            migrationBuilder.DropColumn(
                name: "DoubleConfirm",
                table: "OutOrderMasterItem");

            migrationBuilder.DropColumn(
                name: "NormalReagentStockId",
                table: "OutOrderMasterItem");

            migrationBuilder.DropColumn(
                name: "StockoutAccount",
                table: "OutOrderMasterItem");

            migrationBuilder.RenameColumn(
                name: "DoubleConfirmed",
                table: "OutOrderMasterItem",
                newName: "IsAudit");

            migrationBuilder.AlterColumn<int>(
                name: "ReagentStockId",
                table: "OutOrderMasterItem",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OutOrderMasterItem_ReagentStock_ReagentStockId",
                table: "OutOrderMasterItem",
                column: "ReagentStockId",
                principalTable: "ReagentStock",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
