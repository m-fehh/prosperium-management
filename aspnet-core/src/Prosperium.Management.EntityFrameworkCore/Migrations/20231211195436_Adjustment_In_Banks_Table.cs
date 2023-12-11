using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prosperium.Management.Migrations
{
    /// <inheritdoc />
    public partial class Adjustment_In_Banks_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Origin",
                table: "Pxp_Banks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Compe",
                table: "Pxp_Banks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ispb",
                table: "Pxp_Banks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TradeName",
                table: "Pxp_Banks",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Compe",
                table: "Pxp_Banks");

            migrationBuilder.DropColumn(
                name: "Ispb",
                table: "Pxp_Banks");

            migrationBuilder.DropColumn(
                name: "TradeName",
                table: "Pxp_Banks");

            migrationBuilder.AlterColumn<int>(
                name: "Origin",
                table: "Pxp_Banks",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
