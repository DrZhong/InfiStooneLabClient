using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guoxu.LabManager.Migrations
{
    public partial class _004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LocationStorageAttr_LocationId",
                table: "LocationStorageAttr");

            migrationBuilder.CreateIndex(
                name: "IX_LocationStorageAttr_LocationId_StorageAttr",
                table: "LocationStorageAttr",
                columns: new[] { "LocationId", "StorageAttr" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LocationStorageAttr_LocationId_StorageAttr",
                table: "LocationStorageAttr");

            migrationBuilder.CreateIndex(
                name: "IX_LocationStorageAttr_LocationId",
                table: "LocationStorageAttr",
                column: "LocationId");
        }
    }
}
