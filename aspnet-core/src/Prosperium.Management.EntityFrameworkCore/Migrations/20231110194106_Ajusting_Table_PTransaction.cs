using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prosperium.Management.Migrations
{
    /// <inheritdoc />
    public partial class Ajusting_Table_PTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_P_Transactions_P_Categories_CategoriesId",
                table: "P_Transactions");

            migrationBuilder.DropIndex(
                name: "IX_P_Transactions_CategoriesId",
                table: "P_Transactions");

            migrationBuilder.DropColumn(
                name: "CategoriesId",
                table: "P_Transactions");

            migrationBuilder.DropColumn(
                name: "Paid",
                table: "P_Transactions");

            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                table: "P_Transactions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_P_Transactions_CategoryId",
                table: "P_Transactions",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_P_Transactions_P_Categories_CategoryId",
                table: "P_Transactions",
                column: "CategoryId",
                principalTable: "P_Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_P_Transactions_P_Categories_CategoryId",
                table: "P_Transactions");

            migrationBuilder.DropIndex(
                name: "IX_P_Transactions_CategoryId",
                table: "P_Transactions");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "P_Transactions");

            migrationBuilder.AddColumn<long>(
                name: "CategoriesId",
                table: "P_Transactions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Paid",
                table: "P_Transactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_P_Transactions_CategoriesId",
                table: "P_Transactions",
                column: "CategoriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_P_Transactions_P_Categories_CategoriesId",
                table: "P_Transactions",
                column: "CategoriesId",
                principalTable: "P_Categories",
                principalColumn: "Id");
        }
    }
}
