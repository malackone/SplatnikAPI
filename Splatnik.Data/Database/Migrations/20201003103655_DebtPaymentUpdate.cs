using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Splatnik.Data.Database.Migrations
{
	public partial class DebtPaymentUpdate : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<DateTime>(
				name: "PaymentDate",
				table: "DebtPayments",
				nullable: false,
				defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "PaymentDate",
				table: "DebtPayments");
		}
	}
}
