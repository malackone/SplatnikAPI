using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Splatnik.Data.Database.Migrations
{
	public partial class UpdateExpenseModel : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "IncomeDate",
				table: "Expenses");

			migrationBuilder.AddColumn<DateTime>(
				name: "ExpenseDate",
				table: "Expenses",
				nullable: false,
				defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "ExpenseDate",
				table: "Expenses");

			migrationBuilder.AddColumn<DateTime>(
				name: "IncomeDate",
				table: "Expenses",
				type: "datetime2",
				nullable: false,
				defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
		}
	}
}
