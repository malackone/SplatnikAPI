using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Splatnik.Data.Database.Migrations
{
	public partial class DebtAndDebtPaymentModel : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Expenses_Currency_CurrencyId",
				table: "Expenses");

			migrationBuilder.DropForeignKey(
				name: "FK_Incomes_Currency_CurrencyId",
				table: "Incomes");

			migrationBuilder.DropPrimaryKey(
				name: "PK_Currency",
				table: "Currency");

			migrationBuilder.RenameTable(
				name: "Currency",
				newName: "Currencies");

			migrationBuilder.AddPrimaryKey(
				name: "PK_Currencies",
				table: "Currencies",
				column: "Id");

			migrationBuilder.CreateTable(
				name: "Debts",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					CreatedAt = table.Column<DateTime>(nullable: false),
					Name = table.Column<string>(maxLength: 100, nullable: false),
					Description = table.Column<string>(maxLength: 200, nullable: true),
					InitialValue = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
					BudgetId = table.Column<int>(nullable: false),
					CurrencyId = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Debts", x => x.Id);
					table.ForeignKey(
						name: "FK_Debts_Budgets_BudgetId",
						column: x => x.BudgetId,
						principalTable: "Budgets",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Debts_Currencies_CurrencyId",
						column: x => x.CurrencyId,
						principalTable: "Currencies",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "DebtPayments",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					CreatedAt = table.Column<DateTime>(nullable: false),
					Description = table.Column<string>(maxLength: 200, nullable: true),
					DebtPaymentValue = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
					CurrencyId = table.Column<int>(nullable: false),
					DebtId = table.Column<int>(nullable: false),
					PeriodId = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_DebtPayments", x => x.Id);
					table.ForeignKey(
						name: "FK_DebtPayments_Currencies_CurrencyId",
						column: x => x.CurrencyId,
						principalTable: "Currencies",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_DebtPayments_Debts_DebtId",
						column: x => x.DebtId,
						principalTable: "Debts",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_DebtPayments_Periods_PeriodId",
						column: x => x.PeriodId,
						principalTable: "Periods",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_DebtPayments_CurrencyId",
				table: "DebtPayments",
				column: "CurrencyId");

			migrationBuilder.CreateIndex(
				name: "IX_DebtPayments_DebtId",
				table: "DebtPayments",
				column: "DebtId");

			migrationBuilder.CreateIndex(
				name: "IX_DebtPayments_PeriodId",
				table: "DebtPayments",
				column: "PeriodId");

			migrationBuilder.CreateIndex(
				name: "IX_Debts_BudgetId",
				table: "Debts",
				column: "BudgetId");

			migrationBuilder.CreateIndex(
				name: "IX_Debts_CurrencyId",
				table: "Debts",
				column: "CurrencyId");

			migrationBuilder.AddForeignKey(
				name: "FK_Expenses_Currencies_CurrencyId",
				table: "Expenses",
				column: "CurrencyId",
				principalTable: "Currencies",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_Incomes_Currencies_CurrencyId",
				table: "Incomes",
				column: "CurrencyId",
				principalTable: "Currencies",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Expenses_Currencies_CurrencyId",
				table: "Expenses");

			migrationBuilder.DropForeignKey(
				name: "FK_Incomes_Currencies_CurrencyId",
				table: "Incomes");

			migrationBuilder.DropTable(
				name: "DebtPayments");

			migrationBuilder.DropTable(
				name: "Debts");

			migrationBuilder.DropPrimaryKey(
				name: "PK_Currencies",
				table: "Currencies");

			migrationBuilder.RenameTable(
				name: "Currencies",
				newName: "Currency");

			migrationBuilder.AddPrimaryKey(
				name: "PK_Currency",
				table: "Currency",
				column: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_Expenses_Currency_CurrencyId",
				table: "Expenses",
				column: "CurrencyId",
				principalTable: "Currency",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_Incomes_Currency_CurrencyId",
				table: "Incomes",
				column: "CurrencyId",
				principalTable: "Currency",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}
	}
}
