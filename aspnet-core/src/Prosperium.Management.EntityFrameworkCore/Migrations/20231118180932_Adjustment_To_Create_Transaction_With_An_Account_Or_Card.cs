using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prosperium.Management.Migrations
{
    /// <inheritdoc />
    public partial class Adjustment_To_Create_Transaction_With_An_Account_Or_Card : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "AccountId",
                table: "Pxp_Transactions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "CreditCardId",
                table: "Pxp_Transactions",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pxp_Transactions_CreditCardId",
                table: "Pxp_Transactions",
                column: "CreditCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Cards_CreditCardId",
                table: "Pxp_Transactions",
                column: "CreditCardId",
                principalTable: "Pxp_Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Cards_CreditCardId",
                table: "Pxp_Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Pxp_Transactions_CreditCardId",
                table: "Pxp_Transactions");

            migrationBuilder.DropColumn(
                name: "CreditCardId",
                table: "Pxp_Transactions");

            migrationBuilder.AlterColumn<long>(
                name: "AccountId",
                table: "Pxp_Transactions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
