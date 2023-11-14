using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prosperium.Management.Migrations
{
    /// <inheritdoc />
    public partial class Ajusting_Table_Accounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pxp_Accounts_Pxp_Banks_InstitutionId",
                table: "Pxp_Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Pxp_Accounts_InstitutionId",
                table: "Pxp_Accounts");

            migrationBuilder.AlterColumn<long>(
                name: "InstitutionId",
                table: "Pxp_Accounts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "InstitutionId",
                table: "Pxp_Accounts",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_Pxp_Accounts_InstitutionId",
                table: "Pxp_Accounts",
                column: "InstitutionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pxp_Accounts_Pxp_Banks_InstitutionId",
                table: "Pxp_Accounts",
                column: "InstitutionId",
                principalTable: "Pxp_Banks",
                principalColumn: "Id");
        }
    }
}
