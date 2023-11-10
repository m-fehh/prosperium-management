using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prosperium.Management.Migrations
{
    /// <inheritdoc />
    public partial class Add_Column_TransactionType_In_Categories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TransactionType",
                table: "P_Categories",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TransactionType",
                table: "P_Categories",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32);
        }
    }
}
