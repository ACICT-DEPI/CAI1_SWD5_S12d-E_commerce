using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce___DEPI.Migrations
{
    /// <inheritdoc />
    public partial class updateorderarchive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customers_AddressId",
                table: "Customers");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersArchives_OrderId",
                table: "OrdersArchives",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersArchives_Orders_OrderId",
                table: "OrdersArchives",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Customers_CustomerId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_ShippmentCities_ShippmentCitiesId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdersArchives_Orders_OrderId",
                table: "OrdersArchives");

            migrationBuilder.DropIndex(
                name: "IX_OrdersArchives_OrderId",
                table: "OrdersArchives");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_CustomerId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "OrdersArchives",
                newName: "Shipping_Address_Id");

            migrationBuilder.RenameColumn(
                name: "ShippmentCitiesId",
                table: "Addresses",
                newName: "ShippmentCityId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_ShippmentCitiesId",
                table: "Addresses",
                newName: "IX_Addresses_ShippmentCityId");

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "OrdersArchives",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Customer_id",
                table: "OrdersArchives",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OrdersArchives",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ShippingAdressId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrdersArchives_AddressId",
                table: "OrdersArchives",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_AddressId",
                table: "Customers",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_ShippmentCities_ShippmentCityId",
                table: "Addresses",
                column: "ShippmentCityId",
                principalTable: "ShippmentCities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Addresses_AddressId",
                table: "Customers",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersArchives_Addresses_AddressId",
                table: "OrdersArchives",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");
        }
    }
}
