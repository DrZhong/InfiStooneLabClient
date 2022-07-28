using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guoxu.LabManager.Migrations
{
    public partial class _024 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LatestStockInTime",
                table: "NormalReagentStock",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WarehouseName",
                table: "NormalReagentOperateRecord",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LatestStockInTime",
                table: "NormalReagentStock");

            migrationBuilder.AlterColumn<int>(
                name: "WarehouseName",
                table: "NormalReagentOperateRecord",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
