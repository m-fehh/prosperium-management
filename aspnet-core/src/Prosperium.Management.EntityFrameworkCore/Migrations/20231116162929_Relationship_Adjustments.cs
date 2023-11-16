using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prosperium.Management.Migrations
{
    /// <inheritdoc />
    public partial class Relationship_Adjustments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pxp_Transactions_AccountId",
                table: "Pxp_Transactions");

            migrationBuilder.CreateIndex(
                name: "IX_Pxp_Transactions_AccountId",
                table: "Pxp_Transactions",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pxp_Transactions_AccountId",
                table: "Pxp_Transactions");

            migrationBuilder.CreateIndex(
                name: "IX_Pxp_Transactions_AccountId",
                table: "Pxp_Transactions",
                column: "AccountId",
                unique: true);
        }
    }
}
