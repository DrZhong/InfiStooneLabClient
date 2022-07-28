using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guoxu.LabManager.Migrations
{
    public partial class _042 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reagent_No",
                table: "Reagent");

            migrationBuilder.CreateIndex(
                name: "IX_Reagent_No_WarehouseType",
                table: "Reagent",
                columns: new[] { "No", "WarehouseType" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reagent_No_WarehouseType",
                table: "Reagent");

            migrationBuilder.CreateIndex(
                name: "IX_Reagent_No",
                table: "Reagent",
                column: "No",
                unique: true);
        }
    }
}
