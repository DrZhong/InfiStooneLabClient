using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guoxu.LabManager.Migrations
{
    public partial class _028 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReagentCasNo",
                table: "OutOrderMasterItem",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReagentCnAliasName",
                table: "OutOrderMasterItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReagentCnName",
                table: "OutOrderMasterItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReagentEnName",
                table: "OutOrderMasterItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReagentNo",
                table: "OutOrderMasterItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReagentReagentCatalog",
                table: "OutOrderMasterItem",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReagentCasNo",
                table: "OutOrderMasterItem");

            migrationBuilder.DropColumn(
                name: "ReagentCnAliasName",
                table: "OutOrderMasterItem");

            migrationBuilder.DropColumn(
                name: "ReagentCnName",
                table: "OutOrderMasterItem");

            migrationBuilder.DropColumn(
                name: "ReagentEnName",
                table: "OutOrderMasterItem");

            migrationBuilder.DropColumn(
                name: "ReagentNo",
                table: "OutOrderMasterItem");

            migrationBuilder.DropColumn(
                name: "ReagentReagentCatalog",
                table: "OutOrderMasterItem");
        }
    }
}
