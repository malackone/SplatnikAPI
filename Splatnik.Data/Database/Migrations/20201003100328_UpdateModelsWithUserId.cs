using Microsoft.EntityFrameworkCore.Migrations;

namespace Splatnik.Data.Database.Migrations
{
	public partial class UpdateModelsWithUserId : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
				name: "UserId",
				table: "Periods",
				maxLength: 450,
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<string>(
				name: "UserId",
				table: "Incomes",
				maxLength: 450,
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<string>(
				name: "UserId",
				table: "Expenses",
				maxLength: 450,
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<string>(
				name: "UserId",
				table: "Debts",
				maxLength: 450,
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<string>(
				name: "UserId",
				table: "DebtPayments",
				maxLength: 450,
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<string>(
				name: "UserId",
				table: "Credits",
				maxLength: 450,
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<string>(
				name: "UserId",
				table: "CreditPayments",
				maxLength: 450,
				nullable: false,
				defaultValue: "");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "UserId",
				table: "Periods");

			migrationBuilder.DropColumn(
				name: "UserId",
				table: "Incomes");

			migrationBuilder.DropColumn(
				name: "UserId",
				table: "Expenses");

			migrationBuilder.DropColumn(
				name: "UserId",
				table: "Debts");

			migrationBuilder.DropColumn(
				name: "UserId",
				table: "DebtPayments");

			migrationBuilder.DropColumn(
				name: "UserId",
				table: "Credits");

			migrationBuilder.DropColumn(
				name: "UserId",
				table: "CreditPayments");
		}
	}
}
