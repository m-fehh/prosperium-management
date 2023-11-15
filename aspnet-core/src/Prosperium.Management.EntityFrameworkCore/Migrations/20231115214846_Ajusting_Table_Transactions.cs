using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prosperium.Management.Migrations
{
    /// <inheritdoc />
    public partial class Ajusting_Table_Transactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Accounts_AccountId",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Accounts_AccountId",
                table: "Pxp_Transactions",
                column: "AccountId",
                principalTable: "Pxp_Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Accounts_AccountId",
                table: "Pxp_Transactions");

            migrationBuilder.AlterColumn<long>(
                name: "AccountId",
                table: "Pxp_Transactions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Accounts_AccountId",
                table: "Pxp_Transactions",
                column: "AccountId",
                principalTable: "Pxp_Accounts",
                principalColumn: "Id");
        }
    }
}
