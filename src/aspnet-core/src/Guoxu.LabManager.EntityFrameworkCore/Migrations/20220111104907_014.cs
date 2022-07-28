using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guoxu.LabManager.Migrations
{
    public partial class _014 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SafeAttribute",
                table: "ReagentStock",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SafeAttribute",
                table: "ReagentOperateRecord",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SafeAttribute",
                table: "ReagentStock");

            migrationBuilder.DropColumn(
                name: "SafeAttribute",
                table: "ReagentOperateRecord");
        }
    }
}
