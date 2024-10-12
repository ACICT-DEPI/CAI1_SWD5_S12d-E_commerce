using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce___DEPI.Migrations
{
    /// <inheritdoc />
    public partial class RestoreCustomerIdToOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
       name: "CustomerId",
       table: "Orders",
       type: "int",
       nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict); // Change to Cascade if you want to delete orders when the customer is deleted
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
