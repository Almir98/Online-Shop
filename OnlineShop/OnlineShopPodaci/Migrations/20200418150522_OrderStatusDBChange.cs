using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineShopPodaci.Migrations
{
    public partial class OrderStatusDBChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_order_OrderStatus_OrderStatusID",
                table: "order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderStatus",
                table: "OrderStatus");

            migrationBuilder.RenameTable(
                name: "OrderStatus",
                newName: "orderstatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_orderstatus",
                table: "orderstatus",
                column: "OrderStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_order_orderstatus_OrderStatusID",
                table: "order",
                column: "OrderStatusID",
                principalTable: "orderstatus",
                principalColumn: "OrderStatusId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_order_orderstatus_OrderStatusID",
                table: "order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_orderstatus",
                table: "orderstatus");

            migrationBuilder.RenameTable(
                name: "orderstatus",
                newName: "OrderStatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderStatus",
                table: "OrderStatus",
                column: "OrderStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_order_OrderStatus_OrderStatusID",
                table: "order",
                column: "OrderStatusID",
                principalTable: "OrderStatus",
                principalColumn: "OrderStatusId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
