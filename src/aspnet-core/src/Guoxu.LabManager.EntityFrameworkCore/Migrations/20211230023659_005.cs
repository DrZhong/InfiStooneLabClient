using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guoxu.LabManager.Migrations
{
    public partial class _005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reagent_Warehouse_WarehouseId",
                table: "Reagent");

            migrationBuilder.DropIndex(
                name: "IX_Reagent_WarehouseId",
                table: "Reagent");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "Reagent");

            migrationBuilder.RenameColumn(
                name: "purity",
                table: "Reagent",
                newName: "Purity");

            migrationBuilder.RenameColumn(
                name: "capacityUnit",
                table: "Reagent",
                newName: "CapacityUnit");

            migrationBuilder.RenameColumn(
                name: "capacity",
                table: "Reagent",
                newName: "Capacity");

            migrationBuilder.AlterColumn<string>(
                name: "CapacityUnit",
                table: "Reagent",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Capacity",
                table: "Reagent",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "No",
                table: "Reagent",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EnName",
                table: "Reagent",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CnName",
                table: "Reagent",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ReagentLocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReagentId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReagentLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReagentLocation_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReagentLocation_Reagent_ReagentId",
                        column: x => x.ReagentId,
                        principalTable: "Reagent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReagentLocation_LocationId",
                table: "ReagentLocation",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReagentLocation_ReagentId",
                table: "ReagentLocation",
                column: "ReagentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReagentLocation");

            migrationBuilder.RenameColumn(
                name: "Purity",
                table: "Reagent",
                newName: "purity");

            migrationBuilder.RenameColumn(
                name: "CapacityUnit",
                table: "Reagent",
                newName: "capacityUnit");

            migrationBuilder.RenameColumn(
                name: "Capacity",
                table: "Reagent",
                newName: "capacity");

            migrationBuilder.AlterColumn<string>(
                name: "No",
                table: "Reagent",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512);

            migrationBuilder.AlterColumn<string>(
                name: "EnName",
                table: "Reagent",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)",
                oldMaxLength: 1024,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CnName",
                table: "Reagent",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512);

            migrationBuilder.AlterColumn<string>(
                name: "capacityUnit",
                table: "Reagent",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "capacity",
                table: "Reagent",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WarehouseId",
                table: "Reagent",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reagent_WarehouseId",
                table: "Reagent",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reagent_Warehouse_WarehouseId",
                table: "Reagent",
                column: "WarehouseId",
                principalTable: "Warehouse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
