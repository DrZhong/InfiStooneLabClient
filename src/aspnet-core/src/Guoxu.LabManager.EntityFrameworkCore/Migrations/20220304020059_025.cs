using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guoxu.LabManager.Migrations
{
    public partial class _025 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "No",
                table: "NormalReagentOperateRecord",
                newName: "ReagentPinYinCode");

            migrationBuilder.RenameColumn(
                name: "CnName",
                table: "NormalReagentOperateRecord",
                newName: "ReagentNo");

            migrationBuilder.AddColumn<string>(
                name: "ReagentCasNo",
                table: "NormalReagentOperateRecord",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReagentCnName",
                table: "NormalReagentOperateRecord",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReagentEnName",
                table: "NormalReagentOperateRecord",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReagentReagentCatalog",
                table: "NormalReagentOperateRecord",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReagentCasNo",
                table: "NormalReagentOperateRecord");

            migrationBuilder.DropColumn(
                name: "ReagentCnName",
                table: "NormalReagentOperateRecord");

            migrationBuilder.DropColumn(
                name: "ReagentEnName",
                table: "NormalReagentOperateRecord");

            migrationBuilder.DropColumn(
                name: "ReagentReagentCatalog",
                table: "NormalReagentOperateRecord");

            migrationBuilder.RenameColumn(
                name: "ReagentPinYinCode",
                table: "NormalReagentOperateRecord",
                newName: "No");

            migrationBuilder.RenameColumn(
                name: "ReagentNo",
                table: "NormalReagentOperateRecord",
                newName: "CnName");
        }
    }
}
