using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prosperium.Management.Migrations
{
    /// <inheritdoc />
    public partial class Recreating_The_ProsperiumDb_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_P_Subcategories_P_Categories_CategoryId",
                table: "P_Subcategories");

            migrationBuilder.DropForeignKey(
                name: "FK_P_Tags_P_Transactions_TransactionId",
                table: "P_Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_P_Transactions_P_Accounts_AccountId",
                table: "P_Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_P_Transactions_P_Categories_CategoryId",
                table: "P_Transactions");

            migrationBuilder.DropTable(
                name: "P_Accounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_P_Transactions",
                table: "P_Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_P_Subcategories",
                table: "P_Subcategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_P_Categories",
                table: "P_Categories");

            migrationBuilder.RenameTable(
                name: "P_Transactions",
                newName: "Pxp_Transactions");

            migrationBuilder.RenameTable(
                name: "P_Subcategories",
                newName: "Pxp_Subcategories");

            migrationBuilder.RenameTable(
                name: "P_Categories",
                newName: "Pxp_Categories");

            migrationBuilder.RenameIndex(
                name: "IX_P_Transactions_CategoryId",
                table: "Pxp_Transactions",
                newName: "IX_Pxp_Transactions_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_P_Transactions_AccountId",
                table: "Pxp_Transactions",
                newName: "IX_Pxp_Transactions_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_P_Subcategories_CategoryId",
                table: "Pxp_Subcategories",
                newName: "IX_Pxp_Subcategories_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pxp_Transactions",
                table: "Pxp_Transactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pxp_Subcategories",
                table: "Pxp_Subcategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pxp_Categories",
                table: "Pxp_Categories",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Pxp_Banks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pxp_Banks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pxp_Accounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    InstitutionId = table.Column<long>(type: "bigint", nullable: true),
                    AccountNickname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BalanceAvailable = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AccountType = table.Column<int>(type: "int", nullable: false),
                    MainAccount = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pxp_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pxp_Accounts_Pxp_Banks_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "Pxp_Banks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pxp_Accounts_InstitutionId",
                table: "Pxp_Accounts",
                column: "InstitutionId");

            migrationBuilder.AddForeignKey(
                name: "FK_P_Tags_Pxp_Transactions_TransactionId",
                table: "P_Tags",
                column: "TransactionId",
                principalTable: "Pxp_Transactions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pxp_Subcategories_Pxp_Categories_CategoryId",
                table: "Pxp_Subcategories",
                column: "CategoryId",
                principalTable: "Pxp_Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Accounts_AccountId",
                table: "Pxp_Transactions",
                column: "AccountId",
                principalTable: "Pxp_Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Categories_CategoryId",
                table: "Pxp_Transactions",
                column: "CategoryId",
                principalTable: "Pxp_Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_P_Tags_Pxp_Transactions_TransactionId",
                table: "P_Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Pxp_Subcategories_Pxp_Categories_CategoryId",
                table: "Pxp_Subcategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Accounts_AccountId",
                table: "Pxp_Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Categories_CategoryId",
                table: "Pxp_Transactions");

            migrationBuilder.DropTable(
                name: "Pxp_Accounts");

            migrationBuilder.DropTable(
                name: "Pxp_Banks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pxp_Transactions",
                table: "Pxp_Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pxp_Subcategories",
                table: "Pxp_Subcategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pxp_Categories",
                table: "Pxp_Categories");

            migrationBuilder.RenameTable(
                name: "Pxp_Transactions",
                newName: "P_Transactions");

            migrationBuilder.RenameTable(
                name: "Pxp_Subcategories",
                newName: "P_Subcategories");

            migrationBuilder.RenameTable(
                name: "Pxp_Categories",
                newName: "P_Categories");

            migrationBuilder.RenameIndex(
                name: "IX_Pxp_Transactions_CategoryId",
                table: "P_Transactions",
                newName: "IX_P_Transactions_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Pxp_Transactions_AccountId",
                table: "P_Transactions",
                newName: "IX_P_Transactions_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Pxp_Subcategories_CategoryId",
                table: "P_Subcategories",
                newName: "IX_P_Subcategories_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_P_Transactions",
                table: "P_Transactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_P_Subcategories",
                table: "P_Subcategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_P_Categories",
                table: "P_Categories",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "P_Accounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FinancialInstitutionId = table.Column<long>(type: "bigint", nullable: true),
                    AccountNickname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountType = table.Column<int>(type: "int", nullable: false),
                    BalanceAvailable = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainAccount = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_P_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_P_Accounts_P_Accounts_FinancialInstitutionId",
                        column: x => x.FinancialInstitutionId,
                        principalTable: "P_Accounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_P_Accounts_FinancialInstitutionId",
                table: "P_Accounts",
                column: "FinancialInstitutionId");

            migrationBuilder.AddForeignKey(
                name: "FK_P_Subcategories_P_Categories_CategoryId",
                table: "P_Subcategories",
                column: "CategoryId",
                principalTable: "P_Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_P_Tags_P_Transactions_TransactionId",
                table: "P_Tags",
                column: "TransactionId",
                principalTable: "P_Transactions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_P_Transactions_P_Accounts_AccountId",
                table: "P_Transactions",
                column: "AccountId",
                principalTable: "P_Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_P_Transactions_P_Categories_CategoryId",
                table: "P_Transactions",
                column: "CategoryId",
                principalTable: "P_Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
