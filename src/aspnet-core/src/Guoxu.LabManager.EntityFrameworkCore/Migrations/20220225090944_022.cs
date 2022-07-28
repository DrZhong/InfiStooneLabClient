using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guoxu.LabManager.Migrations
{
    public partial class _022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ClientConfirmed",
                table: "ReagentStock",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DoubleConfirmed",
                table: "ReagentStock",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientConfirmed",
                table: "ReagentStock");

            migrationBuilder.DropColumn(
                name: "DoubleConfirmed",
                table: "ReagentStock");
        }
    }
}
