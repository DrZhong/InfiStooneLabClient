using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guoxu.LabManager.Migrations
{
    public partial class _032 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReagentPurity",
                table: "OutOrderMasterItem",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReagentPurity",
                table: "OutOrderMasterItem");
        }
    }
}
