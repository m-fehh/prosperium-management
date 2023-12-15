using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prosperium.Management.Migrations
{
    /// <inheritdoc />
    public partial class Adding_The_Pxp_Opportunities_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pxp_Opportunities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalQuotas = table.Column<int>(type: "int", nullable: false),
                    QuotasType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InterestRate = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvailableLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Origin = table.Column<int>(type: "int", nullable: false),
                    PluggyOpportunityId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PluggyItemId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pxp_Opportunities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pxp_Opportunities_Pxp_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Pxp_Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pxp_Opportunities_AccountId",
                table: "Pxp_Opportunities",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pxp_Opportunities");
        }
    }
}
