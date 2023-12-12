using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prosperium.Management.Migrations
{
    /// <inheritdoc />
    public partial class AccountId_Is_Now_Nullable_On_CreditCardDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "AccountId",
                table: "Pxp_Cards",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "AccountId",
                table: "Pxp_Cards",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
