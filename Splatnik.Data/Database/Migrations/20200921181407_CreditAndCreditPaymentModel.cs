using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Splatnik.Data.Database.Migrations
{
	public partial class CreditAndCreditPaymentModel : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Credits",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					CreatedAt = table.Column<DateTime>(nullable: false),
					Name = table.Column<string>(maxLength: 100, nullable: false),
					Description = table.Column<string>(maxLength: 200, nullable: true),
					BankName = table.Column<string>(maxLength: 100, nullable: true),
					BankAccountNumber = table.Column<string>(maxLength: 40, nullable: true),
					ContractNumber = table.Column<string>(maxLength: 100, nullable: true),
					InitNoOfPayments = table.Column<int>(nullable: false),
					InitialCreditValue = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
					PaymentDayOfMonth = table.Column<int>(nullable: false),
					CurrencyId = table.Column<int>(nullable: false),
					BudgetId = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Credits", x => x.Id);
					table.ForeignKey(
						name: "FK_Credits_Budgets_BudgetId",
						column: x => x.BudgetId,
						principalTable: "Budgets",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Credits_Currencies_CurrencyId",
						column: x => x.CurrencyId,
						principalTable: "Currencies",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "CreditPayments",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					CreatedAt = table.Column<DateTime>(nullable: false),
					PaidAt = table.Column<DateTime>(nullable: false),
					PlannedDateOfPayment = table.Column<DateTime>(nullable: false),
					PaymentValue = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
					IsPaid = table.Column<bool>(nullable: false),
					PeriodId = table.Column<int>(nullable: false),
					CurrencyId = table.Column<int>(nullable: false),
					CreditId = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_CreditPayments", x => x.Id);
					table.ForeignKey(
						name: "FK_CreditPayments_Credits_CreditId",
						column: x => x.CreditId,
						principalTable: "Credits",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_CreditPayments_Currencies_CurrencyId",
						column: x => x.CurrencyId,
						principalTable: "Currencies",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_CreditPayments_Periods_PeriodId",
						column: x => x.PeriodId,
						principalTable: "Periods",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_CreditPayments_CreditId",
				table: "CreditPayments",
				column: "CreditId");

			migrationBuilder.CreateIndex(
				name: "IX_CreditPayments_CurrencyId",
				table: "CreditPayments",
				column: "CurrencyId");

			migrationBuilder.CreateIndex(
				name: "IX_CreditPayments_PeriodId",
				table: "CreditPayments",
				column: "PeriodId");

			migrationBuilder.CreateIndex(
				name: "IX_Credits_BudgetId",
				table: "Credits",
				column: "BudgetId");

			migrationBuilder.CreateIndex(
				name: "IX_Credits_CurrencyId",
				table: "Credits",
				column: "CurrencyId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "CreditPayments");

			migrationBuilder.DropTable(
				name: "Credits");
		}
	}
}
