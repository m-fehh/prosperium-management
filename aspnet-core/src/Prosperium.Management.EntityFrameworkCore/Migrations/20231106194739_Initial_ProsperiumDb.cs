using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prosperium.Management.Migrations
{
    /// <inheritdoc />
    public partial class Initial_ProsperiumDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TargetNotifiers",
                table: "AbpNotificationSubscriptions",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "P_Accounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    AccountNickname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BalanceAvailable = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AccountType = table.Column<int>(type: "int", nullable: false),
                    FinancialInstitutionId = table.Column<long>(type: "bigint", nullable: true),
                    MainAccount = table.Column<bool>(type: "bit", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "P_Subcategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_P_Subcategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "P_Categories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubcategoryId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_P_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_P_Categories_P_Subcategories_SubcategoryId",
                        column: x => x.SubcategoryId,
                        principalTable: "P_Subcategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "P_Transactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ExpenseValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoriesId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentType = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    PaymentTerm = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Paid = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_P_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_P_Transactions_P_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "P_Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_P_Transactions_P_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "P_Categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "P_Tags",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_P_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_P_Tags_P_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "P_Transactions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_P_Accounts_FinancialInstitutionId",
                table: "P_Accounts",
                column: "FinancialInstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_P_Categories_SubcategoryId",
                table: "P_Categories",
                column: "SubcategoryId",
                unique: true,
                filter: "[SubcategoryId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_P_Tags_TransactionId",
                table: "P_Tags",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_P_Transactions_AccountId",
                table: "P_Transactions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_P_Transactions_CategoriesId",
                table: "P_Transactions",
                column: "CategoriesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "P_Tags");

            migrationBuilder.DropTable(
                name: "P_Transactions");

            migrationBuilder.DropTable(
                name: "P_Accounts");

            migrationBuilder.DropTable(
                name: "P_Categories");

            migrationBuilder.DropTable(
                name: "P_Subcategories");

            migrationBuilder.DropColumn(
                name: "TargetNotifiers",
                table: "AbpNotificationSubscriptions");
        }
    }
}
