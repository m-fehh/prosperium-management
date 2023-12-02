using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prosperium.Management.Migrations
{
    /// <inheritdoc />
    public partial class Added_New_Column_In_Table_Banks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Origin",
                table: "Pxp_Cards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PluggyCreditCardId",
                table: "Pxp_Cards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PluggyItemId",
                table: "Pxp_Cards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Origin",
                table: "Pxp_Banks",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "PluggyAccountId",
                table: "Pxp_Accounts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Origin",
                table: "Pxp_Cards");

            migrationBuilder.DropColumn(
                name: "PluggyCreditCardId",
                table: "Pxp_Cards");

            migrationBuilder.DropColumn(
                name: "PluggyItemId",
                table: "Pxp_Cards");

            migrationBuilder.DropColumn(
                name: "Origin",
                table: "Pxp_Banks");

            migrationBuilder.DropColumn(
                name: "PluggyAccountId",
                table: "Pxp_Accounts");
        }
    }
}
