using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prosperium.Management.Migrations
{
    /// <inheritdoc />
    public partial class Correcting_Tag_Table_Name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_P_Tags_Pxp_Transactions_TransactionId",
                table: "P_Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_P_Tags",
                table: "P_Tags");

            migrationBuilder.RenameTable(
                name: "P_Tags",
                newName: "Pxp_Tags");

            migrationBuilder.RenameIndex(
                name: "IX_P_Tags_TransactionId",
                table: "Pxp_Tags",
                newName: "IX_Pxp_Tags_TransactionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pxp_Tags",
                table: "Pxp_Tags",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pxp_Tags_Pxp_Transactions_TransactionId",
                table: "Pxp_Tags",
                column: "TransactionId",
                principalTable: "Pxp_Transactions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pxp_Tags_Pxp_Transactions_TransactionId",
                table: "Pxp_Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pxp_Tags",
                table: "Pxp_Tags");

            migrationBuilder.RenameTable(
                name: "Pxp_Tags",
                newName: "P_Tags");

            migrationBuilder.RenameIndex(
                name: "IX_Pxp_Tags_TransactionId",
                table: "P_Tags",
                newName: "IX_P_Tags_TransactionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_P_Tags",
                table: "P_Tags",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_P_Tags_Pxp_Transactions_TransactionId",
                table: "P_Tags",
                column: "TransactionId",
                principalTable: "Pxp_Transactions",
                principalColumn: "Id");
        }
    }
}
