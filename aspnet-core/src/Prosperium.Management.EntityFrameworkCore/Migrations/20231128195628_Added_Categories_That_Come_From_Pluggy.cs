using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prosperium.Management.Migrations
{
    /// <inheritdoc />
    public partial class Added_Categories_That_Come_From_Pluggy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pxp_Pluggy_Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pluggy_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pluggy_Category_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pluggy_Category_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pluggy_Category_Name_Translated = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pluggy_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pluggy_Description_Translated = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prosperium_Category_Id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pxp_Pluggy_Categories", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pxp_Pluggy_Categories");
        }
    }
}
