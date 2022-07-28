using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guoxu.LabManager.Migrations
{
    public partial class _039 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNoticed",
                table: "ReagentStock",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "LatestStockOutUserId",
                table: "ReagentStock",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountLimit",
                table: "Location",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNoticed",
                table: "ReagentStock");

            migrationBuilder.DropColumn(
                name: "LatestStockOutUserId",
                table: "ReagentStock");

            migrationBuilder.DropColumn(
                name: "CountLimit",
                table: "Location");
        }
    }
}
