using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineShopPodaci.Migrations
{
    public partial class DatabaseNormalization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderdetails_user_UserID",
                table: "orderdetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_orderdetails",
                table: "orderdetails");

            migrationBuilder.DropIndex(
                name: "IX_orderdetails_UserID",
                table: "orderdetails");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "orderdetails");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "order",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_orderdetails",
                table: "orderdetails",
                columns: new[] { "ProductID", "OrderID" });

            migrationBuilder.CreateIndex(
                name: "IX_order_UserID",
                table: "order",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_order_user_UserID",
                table: "order",
                column: "UserID",
                principalTable: "user",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_order_user_UserID",
                table: "order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_orderdetails",
                table: "orderdetails");

            migrationBuilder.DropIndex(
                name: "IX_order_UserID",
                table: "order");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "order");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "orderdetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_orderdetails",
                table: "orderdetails",
                columns: new[] { "ProductID", "UserID", "OrderID" });

            migrationBuilder.CreateIndex(
                name: "IX_orderdetails_UserID",
                table: "orderdetails",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_orderdetails_user_UserID",
                table: "orderdetails",
                column: "UserID",
                principalTable: "user",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
