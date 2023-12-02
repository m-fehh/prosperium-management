using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prosperium.Management.Migrations
{
    /// <inheritdoc />
    public partial class Adjust_Origin_Destination_Screen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pxp_Pluggy_Categories");

            migrationBuilder.AddColumn<int>(
                name: "Origin",
                table: "Pxp_Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PluggyItemId",
                table: "Pxp_Accounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Pxp_OriginDestination",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginPortal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginValueId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginValueName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DestinationPortal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DestinationValueId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DestinationValueName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pxp_OriginDestination", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pxp_OriginDestination");

            migrationBuilder.DropColumn(
                name: "Origin",
                table: "Pxp_Accounts");

            migrationBuilder.DropColumn(
                name: "PluggyItemId",
                table: "Pxp_Accounts");

            migrationBuilder.CreateTable(
                name: "Pxp_Pluggy_Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pluggy_Category_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pluggy_Category_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pluggy_Category_Name_Translated = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pluggy_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pluggy_Description_Translated = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pluggy_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prosperium_Category_Id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pxp_Pluggy_Categories", x => x.Id);
                });
        }
    }
}
