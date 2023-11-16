using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prosperium.Management.Migrations
{
    /// <inheritdoc />
    public partial class New_Relationship_Adjustments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pxp_Accounts_Pxp_Banks_BankId",
                table: "Pxp_Accounts");

            migrationBuilder.AddForeignKey(
                name: "FK_Pxp_Accounts_Pxp_Banks_BankId",
                table: "Pxp_Accounts",
                column: "BankId",
                principalTable: "Pxp_Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pxp_Accounts_Pxp_Banks_BankId",
                table: "Pxp_Accounts");

            migrationBuilder.AddForeignKey(
                name: "FK_Pxp_Accounts_Pxp_Banks_BankId",
                table: "Pxp_Accounts",
                column: "BankId",
                principalTable: "Pxp_Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
