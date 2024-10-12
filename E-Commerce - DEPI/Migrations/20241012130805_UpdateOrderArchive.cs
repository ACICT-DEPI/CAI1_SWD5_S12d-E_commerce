using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce___DEPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderArchive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderArchives",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderArchives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderArchives_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderArchives_OrderId",
                table: "OrderArchives",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderArchives");

            migrationBuilder.CreateTable(
                name: "OrderedItemsArchives",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderedItemsArchives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderedItemsArchives_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderedItemsArchives_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrdersArchives",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersArchives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdersArchives_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrdersArchives_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderedItemsArchives_OrderId",
                table: "OrderedItemsArchives",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedItemsArchives_ProductId",
                table: "OrderedItemsArchives",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersArchives_CustomerId",
                table: "OrdersArchives",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersArchives_OrderId",
                table: "OrdersArchives",
                column: "OrderId");
        }
    }
}
