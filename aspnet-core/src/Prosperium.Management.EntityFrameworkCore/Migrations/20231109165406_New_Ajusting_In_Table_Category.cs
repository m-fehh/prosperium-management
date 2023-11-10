using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prosperium.Management.Migrations
{
    /// <inheritdoc />
    public partial class New_Ajusting_In_Table_Category : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_P_Categories_P_Subcategories_SubcategoryId",
                table: "P_Categories");

            migrationBuilder.DropIndex(
                name: "IX_P_Categories_SubcategoryId",
                table: "P_Categories");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "P_Subcategories");

            migrationBuilder.DropColumn(
                name: "SubcategoryId",
                table: "P_Categories");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "P_Categories");

            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                table: "P_Subcategories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_P_Subcategories_CategoryId",
                table: "P_Subcategories",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_P_Subcategories_P_Categories_CategoryId",
                table: "P_Subcategories",
                column: "CategoryId",
                principalTable: "P_Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_P_Subcategories_P_Categories_CategoryId",
                table: "P_Subcategories");

            migrationBuilder.DropIndex(
                name: "IX_P_Subcategories_CategoryId",
                table: "P_Subcategories");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "P_Subcategories");

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "P_Subcategories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SubcategoryId",
                table: "P_Categories",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "P_Categories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_P_Categories_SubcategoryId",
                table: "P_Categories",
                column: "SubcategoryId",
                unique: true,
                filter: "[SubcategoryId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_P_Categories_P_Subcategories_SubcategoryId",
                table: "P_Categories",
                column: "SubcategoryId",
                principalTable: "P_Subcategories",
                principalColumn: "Id");
        }
    }
}
