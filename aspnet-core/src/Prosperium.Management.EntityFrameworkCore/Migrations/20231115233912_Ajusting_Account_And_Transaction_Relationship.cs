using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prosperium.Management.Migrations
{
    /// <inheritdoc />
    public partial class Ajusting_Account_And_Transaction_Relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pxp_Transactions_AccountId",
                table: "Pxp_Transactions");

            migrationBuilder.RenameColumn(
                name: "InstitutionId",
                table: "Pxp_Accounts",
                newName: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Pxp_Transactions_AccountId",
                table: "Pxp_Transactions",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pxp_Accounts_BankId",
                table: "Pxp_Accounts",
                column: "BankId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pxp_Accounts_Pxp_Banks_BankId",
                table: "Pxp_Accounts",
                column: "BankId",
                principalTable: "Pxp_Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pxp_Accounts_Pxp_Banks_BankId",
                table: "Pxp_Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Pxp_Transactions_AccountId",
                table: "Pxp_Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Pxp_Accounts_BankId",
                table: "Pxp_Accounts");

            migrationBuilder.RenameColumn(
                name: "BankId",
                table: "Pxp_Accounts",
                newName: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Pxp_Transactions_AccountId",
                table: "Pxp_Transactions",
                column: "AccountId");
        }
    }
}
