using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guoxu.LabManager.Migrations
{
    public partial class _027 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OutOrderMasterItem_NormalReagentStock_NormalReagentStockId",
                table: "OutOrderMasterItem");

            migrationBuilder.DropIndex(
                name: "IX_OutOrderMasterItem_NormalReagentStockId",
                table: "OutOrderMasterItem");

            migrationBuilder.RenameColumn(
                name: "NormalReagentStockId",
                table: "OutOrderMasterItem",
                newName: "ReagentId");

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "OutOrderMasterItem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationName",
                table: "OutOrderMasterItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WarehouseName",
                table: "OutOrder",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "NormalReagentLockedStock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReagentId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    LockAccount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NormalReagentLockedStock", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NormalReagentLockedStock");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "OutOrderMasterItem");

            migrationBuilder.DropColumn(
                name: "LocationName",
                table: "OutOrderMasterItem");

            migrationBuilder.RenameColumn(
                name: "ReagentId",
                table: "OutOrderMasterItem",
                newName: "NormalReagentStockId");

            migrationBuilder.AlterColumn<int>(
                name: "WarehouseName",
                table: "OutOrder",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutOrderMasterItem_NormalReagentStockId",
                table: "OutOrderMasterItem",
                column: "NormalReagentStockId");

            migrationBuilder.AddForeignKey(
                name: "FK_OutOrderMasterItem_NormalReagentStock_NormalReagentStockId",
                table: "OutOrderMasterItem",
                column: "NormalReagentStockId",
                principalTable: "NormalReagentStock",
                principalColumn: "Id");
        }
    }
}
