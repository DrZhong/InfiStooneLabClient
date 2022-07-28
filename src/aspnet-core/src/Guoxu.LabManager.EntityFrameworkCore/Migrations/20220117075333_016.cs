using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guoxu.LabManager.Migrations
{
    public partial class _016 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CasNo",
                table: "ReagentStock",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CasNo",
                table: "ReagentStock");
        }
    }
}
