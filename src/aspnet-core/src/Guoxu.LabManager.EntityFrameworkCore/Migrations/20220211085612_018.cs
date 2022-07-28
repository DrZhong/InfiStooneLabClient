using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guoxu.LabManager.Migrations
{
    public partial class _018 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NormalReagentOperateRecord",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BarCode = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    LocationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    Capacity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CapacityUnit = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    BatchNo = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    OperateType = table.Column<int>(type: "int", nullable: false),
                    ReagentId = table.Column<int>(type: "int", nullable: false),
                    CreateUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    Hour = table.Column<int>(type: "int", nullable: false),
                    Minute = table.Column<int>(type: "int", nullable: false),
                    Week = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NormalReagentOperateRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NormalReagentOperateRecord_Reagent_ReagentId",
                        column: x => x.ReagentId,
                        principalTable: "Reagent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NormalReagentOperateRecord_Warehouse_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NormalReagentStock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BarCode = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    BatchNo = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ProductionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpirationMonth = table.Column<int>(type: "int", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    LocationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    SupplierCompanyId = table.Column<int>(type: "int", nullable: true),
                    SupplierCompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductionCompanyId = table.Column<int>(type: "int", nullable: true),
                    ProductionCompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockStatus = table.Column<int>(type: "int", nullable: false),
                    StockInTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LatestStockInUserName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    LatestStockOutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LatestStockOutUserName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ReagentId = table.Column<int>(type: "int", nullable: false),
                    PinYinCode = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    CasNo = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NormalReagentStock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NormalReagentStock_Company_ProductionCompanyId",
                        column: x => x.ProductionCompanyId,
                        principalTable: "Company",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NormalReagentStock_Company_SupplierCompanyId",
                        column: x => x.SupplierCompanyId,
                        principalTable: "Company",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NormalReagentStock_Reagent_ReagentId",
                        column: x => x.ReagentId,
                        principalTable: "Reagent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NormalReagentStock_Warehouse_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NormalReagentOperateRecord_ReagentId",
                table: "NormalReagentOperateRecord",
                column: "ReagentId");

            migrationBuilder.CreateIndex(
                name: "IX_NormalReagentOperateRecord_WarehouseId",
                table: "NormalReagentOperateRecord",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_NormalReagentStock_ProductionCompanyId",
                table: "NormalReagentStock",
                column: "ProductionCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_NormalReagentStock_ReagentId",
                table: "NormalReagentStock",
                column: "ReagentId");

            migrationBuilder.CreateIndex(
                name: "IX_NormalReagentStock_SupplierCompanyId",
                table: "NormalReagentStock",
                column: "SupplierCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_NormalReagentStock_WarehouseId",
                table: "NormalReagentStock",
                column: "WarehouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NormalReagentOperateRecord");

            migrationBuilder.DropTable(
                name: "NormalReagentStock");
        }
    }
}
