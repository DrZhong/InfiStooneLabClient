using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guoxu.LabManager.Migrations
{
    public partial class _019 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Capacity",
                table: "NormalReagentStock",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CapacityUnit",
                table: "NormalReagentStock",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreateUserName",
                table: "NormalReagentStock",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "NormalReagentStock");

            migrationBuilder.DropColumn(
                name: "CapacityUnit",
                table: "NormalReagentStock");

            migrationBuilder.DropColumn(
                name: "CreateUserName",
                table: "NormalReagentStock");
        }
    }
}
