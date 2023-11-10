using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prosperium.Management.Migrations
{
    /// <inheritdoc />
    public partial class Added_Column_TransactionType_In_Categories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransactionType",
                table: "P_Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "P_Categories");
        }
    }
}
