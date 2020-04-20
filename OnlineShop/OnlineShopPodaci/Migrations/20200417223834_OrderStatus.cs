using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineShopPodaci.Migrations
{
    public partial class OrderStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsShipped",
                table: "order");

            migrationBuilder.AddColumn<int>(
                name: "OrderStatusID",
                table: "order",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OrderStatus",
                columns: table => new
                {
                    OrderStatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatus", x => x.OrderStatusId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_order_OrderStatusID",
                table: "order",
                column: "OrderStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_order_OrderStatus_OrderStatusID",
                table: "order",
                column: "OrderStatusID",
                principalTable: "OrderStatus",
                principalColumn: "OrderStatusId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_order_OrderStatus_OrderStatusID",
                table: "order");

            migrationBuilder.DropTable(
                name: "OrderStatus");

            migrationBuilder.DropIndex(
                name: "IX_order_OrderStatusID",
                table: "order");

            migrationBuilder.DropColumn(
                name: "OrderStatusID",
                table: "order");

            migrationBuilder.AddColumn<bool>(
                name: "IsShipped",
                table: "order",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
