using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prosperium.Management.Migrations
{
    /// <inheritdoc />
    public partial class Ajusting_CreditCard_Collection_In_Account : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pxp_Cards_AccountId",
                table: "Pxp_Cards");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Pxp_Cards",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Pxp_Cards_AccountId",
                table: "Pxp_Cards",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pxp_Cards_AccountId",
                table: "Pxp_Cards");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Pxp_Cards");

            migrationBuilder.CreateIndex(
                name: "IX_Pxp_Cards_AccountId",
                table: "Pxp_Cards",
                column: "AccountId",
                unique: true);
        }
    }
}
