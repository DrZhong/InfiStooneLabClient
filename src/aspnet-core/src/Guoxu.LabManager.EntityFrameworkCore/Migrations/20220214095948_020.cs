using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guoxu.LabManager.Migrations
{
    public partial class _020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NormalReagentOperateRecord_Reagent_ReagentId",
                table: "NormalReagentOperateRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_NormalReagentOperateRecord_Warehouse_WarehouseId",
                table: "NormalReagentOperateRecord");

            migrationBuilder.DropIndex(
                name: "IX_NormalReagentOperateRecord_ReagentId",
                table: "NormalReagentOperateRecord");

            migrationBuilder.DropIndex(
                name: "IX_NormalReagentOperateRecord_WarehouseId",
                table: "NormalReagentOperateRecord");

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "NormalReagentStock",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CnName",
                table: "NormalReagentOperateRecord",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "No",
                table: "NormalReagentOperateRecord",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NormalReagentStockId",
                table: "NormalReagentOperateRecord",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OperateAmount",
                table: "NormalReagentOperateRecord",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WarehouseName",
                table: "NormalReagentOperateRecord",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_NormalReagentOperateRecord_NormalReagentStockId",
                table: "NormalReagentOperateRecord",
                column: "NormalReagentStockId");

            migrationBuilder.AddForeignKey(
                name: "FK_NormalReagentOperateRecord_NormalReagentStock_NormalReagentStockId",
                table: "NormalReagentOperateRecord",
                column: "NormalReagentStockId",
                principalTable: "NormalReagentStock",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NormalReagentOperateRecord_NormalReagentStock_NormalReagentStockId",
                table: "NormalReagentOperateRecord");

            migrationBuilder.DropIndex(
                name: "IX_NormalReagentOperateRecord_NormalReagentStockId",
                table: "NormalReagentOperateRecord");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "NormalReagentStock");

            migrationBuilder.DropColumn(
                name: "CnName",
                table: "NormalReagentOperateRecord");

            migrationBuilder.DropColumn(
                name: "No",
                table: "NormalReagentOperateRecord");

            migrationBuilder.DropColumn(
                name: "NormalReagentStockId",
                table: "NormalReagentOperateRecord");

            migrationBuilder.DropColumn(
                name: "OperateAmount",
                table: "NormalReagentOperateRecord");

            migrationBuilder.DropColumn(
                name: "WarehouseName",
                table: "NormalReagentOperateRecord");

            migrationBuilder.CreateIndex(
                name: "IX_NormalReagentOperateRecord_ReagentId",
                table: "NormalReagentOperateRecord",
                column: "ReagentId");

            migrationBuilder.CreateIndex(
                name: "IX_NormalReagentOperateRecord_WarehouseId",
                table: "NormalReagentOperateRecord",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_NormalReagentOperateRecord_Reagent_ReagentId",
                table: "NormalReagentOperateRecord",
                column: "ReagentId",
                principalTable: "Reagent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NormalReagentOperateRecord_Warehouse_WarehouseId",
                table: "NormalReagentOperateRecord",
                column: "WarehouseId",
                principalTable: "Warehouse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
