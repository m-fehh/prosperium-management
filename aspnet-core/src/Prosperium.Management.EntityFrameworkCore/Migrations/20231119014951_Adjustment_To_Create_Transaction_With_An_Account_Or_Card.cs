using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prosperium.Management.Migrations
{
    /// <inheritdoc />
    public partial class Adjustment_To_Create_Transaction_With_An_Account_Or_Card : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Accounts_AccountId",
                table: "Pxp_Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Categories_CategoryId",
                table: "Pxp_Transactions");

            migrationBuilder.AlterColumn<long>(
                name: "AccountId",
                table: "Pxp_Transactions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "CreditCardId",
                table: "Pxp_Transactions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentInstallment",
                table: "Pxp_Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Installments",
                table: "Pxp_Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Pxp_Flags",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IconPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pxp_Flags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pxp_Cards",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CardName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    FlagCardId = table.Column<long>(type: "bigint", nullable: false),
                    Limit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DueDay = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pxp_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pxp_Cards_Pxp_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Pxp_Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pxp_Cards_Pxp_Flags_FlagCardId",
                        column: x => x.FlagCardId,
                        principalTable: "Pxp_Flags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pxp_Transactions_CreditCardId",
                table: "Pxp_Transactions",
                column: "CreditCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Pxp_Cards_AccountId",
                table: "Pxp_Cards",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pxp_Cards_FlagCardId",
                table: "Pxp_Cards",
                column: "FlagCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Accounts_AccountId",
                table: "Pxp_Transactions",
                column: "AccountId",
                principalTable: "Pxp_Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Cards_CreditCardId",
                table: "Pxp_Transactions",
                column: "CreditCardId",
                principalTable: "Pxp_Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Categories_CategoryId",
                table: "Pxp_Transactions",
                column: "CategoryId",
                principalTable: "Pxp_Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Accounts_AccountId",
                table: "Pxp_Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Cards_CreditCardId",
                table: "Pxp_Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Categories_CategoryId",
                table: "Pxp_Transactions");

            migrationBuilder.DropTable(
                name: "Pxp_Cards");

            migrationBuilder.DropTable(
                name: "Pxp_Flags");

            migrationBuilder.DropIndex(
                name: "IX_Pxp_Transactions_CreditCardId",
                table: "Pxp_Transactions");

            migrationBuilder.DropColumn(
                name: "CreditCardId",
                table: "Pxp_Transactions");

            migrationBuilder.DropColumn(
                name: "CurrentInstallment",
                table: "Pxp_Transactions");

            migrationBuilder.DropColumn(
                name: "Installments",
                table: "Pxp_Transactions");

            migrationBuilder.AlterColumn<long>(
                name: "AccountId",
                table: "Pxp_Transactions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Accounts_AccountId",
                table: "Pxp_Transactions",
                column: "AccountId",
                principalTable: "Pxp_Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pxp_Transactions_Pxp_Categories_CategoryId",
                table: "Pxp_Transactions",
                column: "CategoryId",
                principalTable: "Pxp_Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
