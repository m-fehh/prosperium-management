using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prosperium.Management.Migrations
{
    /// <inheritdoc />
    public partial class Cascade_Exclusion_Adjustment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pxp_Cards_Pxp_Accounts_AccountId",
                table: "Pxp_Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Accounts_AccountId",
                table: "Pxp_Transactions");

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
                name: "FK_Pxp_Transactions_Pxp_Accounts_AccountId",
                table: "Pxp_Transactions",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pxp_Cards_Pxp_Accounts_AccountId",
                table: "Pxp_Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Accounts_AccountId",
                table: "Pxp_Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Cards_CreditCardId",
                table: "Pxp_Transactions");

            migrationBuilder.AddForeignKey(
                name: "FK_Pxp_Cards_Pxp_Accounts_AccountId",
                table: "Pxp_Cards",
                column: "AccountId",
                principalTable: "Pxp_Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Accounts_AccountId",
                table: "Pxp_Transactions",
                column: "AccountId",
                principalTable: "Pxp_Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Cards_CreditCardId",
                table: "Pxp_Transactions",
                column: "CreditCardId",
                principalTable: "Pxp_Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
