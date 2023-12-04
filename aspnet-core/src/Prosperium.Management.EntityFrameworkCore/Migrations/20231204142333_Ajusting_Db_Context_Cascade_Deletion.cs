using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prosperium.Management.Migrations
{
    /// <inheritdoc />
    public partial class Ajusting_Db_Context_Cascade_Deletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pxp_Cards_Pxp_Accounts_AccountId",
                table: "Pxp_Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Cards_CreditCardId",
                table: "Pxp_Transactions");

            migrationBuilder.AddForeignKey(
                name: "FK_Pxp_Cards_Pxp_Accounts_AccountId",
                table: "Pxp_Cards",
                column: "AccountId",
                principalTable: "Pxp_Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Cards_CreditCardId",
                table: "Pxp_Transactions",
                column: "CreditCardId",
                principalTable: "Pxp_Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pxp_Cards_Pxp_Accounts_AccountId",
                table: "Pxp_Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Cards_CreditCardId",
                table: "Pxp_Transactions");

            migrationBuilder.AddForeignKey(
                name: "FK_Pxp_Cards_Pxp_Accounts_AccountId",
                table: "Pxp_Cards",
                column: "AccountId",
                principalTable: "Pxp_Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Cards_CreditCardId",
                table: "Pxp_Transactions",
                column: "CreditCardId",
                principalTable: "Pxp_Cards",
                principalColumn: "Id");
        }
    }
}
