using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Splatnik.Data.Database.Migrations
{
	public partial class ComplexModelUpdate : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<DateTime>(
				name: "UpdatedAt",
				table: "Periods",
				nullable: false,
				defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

			migrationBuilder.AddColumn<DateTime>(
				name: "UpdatedAt",
				table: "Incomes",
				nullable: false,
				defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

			migrationBuilder.AddColumn<DateTime>(
				name: "UpdatedAt",
				table: "Expenses",
				nullable: false,
				defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

			migrationBuilder.AddColumn<DateTime>(
				name: "UpdatedAt",
				table: "Debts",
				nullable: false,
				defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

			migrationBuilder.AddColumn<DateTime>(
				name: "UpdatedAt",
				table: "DebtPayments",
				nullable: false,
				defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

			migrationBuilder.AddColumn<DateTime>(
				name: "CreatedAt",
				table: "Currencies",
				nullable: false,
				defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

			migrationBuilder.AddColumn<DateTime>(
				name: "UpdatedAt",
				table: "Currencies",
				nullable: false,
				defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

			migrationBuilder.AddColumn<DateTime>(
				name: "UpdatedAt",
				table: "Credits",
				nullable: false,
				defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

			migrationBuilder.AddColumn<DateTime>(
				name: "UpdatedAt",
				table: "CreditPayments",
				nullable: false,
				defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "UpdatedAt",
				table: "Periods");

			migrationBuilder.DropColumn(
				name: "UpdatedAt",
				table: "Incomes");

			migrationBuilder.DropColumn(
				name: "UpdatedAt",
				table: "Expenses");

			migrationBuilder.DropColumn(
				name: "UpdatedAt",
				table: "Debts");

			migrationBuilder.DropColumn(
				name: "UpdatedAt",
				table: "DebtPayments");

			migrationBuilder.DropColumn(
				name: "CreatedAt",
				table: "Currencies");

			migrationBuilder.DropColumn(
				name: "UpdatedAt",
				table: "Currencies");

			migrationBuilder.DropColumn(
				name: "UpdatedAt",
				table: "Credits");

			migrationBuilder.DropColumn(
				name: "UpdatedAt",
				table: "CreditPayments");
		}
	}
}
