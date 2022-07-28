using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guoxu.LabManager.Migrations
{
    public partial class _008 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReagentOperateRecord",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReagentOperateRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReagentOperateRecord_Reagent_ReagentId",
                        column: x => x.ReagentId,
                        principalTable: "Reagent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReagentOperateRecord_Warehouse_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReagentStock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BarCode = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    BatchNo = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    LocationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    Capacity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CapacityUnit = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    SupplierCompanyId = table.Column<int>(type: "int", nullable: true),
                    SupplierCompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductionCompanyId = table.Column<int>(type: "int", nullable: true),
                    ProductionCompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockStatus = table.Column<int>(type: "int", nullable: false),
                    FirstStockInTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LatestStockInTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LatestStockInUserName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    LatestStockOutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LatestStockOutUserName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    RetrieveTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RetrieveUserName = table.Column<DateTime>(type: "datetime2", maxLength: 512, nullable: true),
                    ReagentId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUserName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReagentStock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReagentStock_Company_ProductionCompanyId",
                        column: x => x.ProductionCompanyId,
                        principalTable: "Company",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReagentStock_Company_SupplierCompanyId",
                        column: x => x.SupplierCompanyId,
                        principalTable: "Company",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReagentStock_Reagent_ReagentId",
                        column: x => x.ReagentId,
                        principalTable: "Reagent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReagentStock_Warehouse_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReagentStockHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReagentStockId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUserName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReagentStockHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReagentStockHistory_ReagentStock_ReagentStockId",
                        column: x => x.ReagentStockId,
                        principalTable: "ReagentStock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReagentOperateRecord_ReagentId",
                table: "ReagentOperateRecord",
                column: "ReagentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReagentOperateRecord_WarehouseId",
                table: "ReagentOperateRecord",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_ReagentStock_ProductionCompanyId",
                table: "ReagentStock",
                column: "ProductionCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ReagentStock_ReagentId",
                table: "ReagentStock",
                column: "ReagentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReagentStock_SupplierCompanyId",
                table: "ReagentStock",
                column: "SupplierCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ReagentStock_WarehouseId",
                table: "ReagentStock",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_ReagentStockHistory_ReagentStockId",
                table: "ReagentStockHistory",
                column: "ReagentStockId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReagentOperateRecord");

            migrationBuilder.DropTable(
                name: "ReagentStockHistory");

            migrationBuilder.DropTable(
                name: "ReagentStock");
        }
    }
}
