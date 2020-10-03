using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Splatnik.Data.Database.Migrations
{
    public partial class UpdateCreditModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentDayOfMonth",
                table: "Credits");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "CreditPayments");

            migrationBuilder.DropColumn(
                name: "PaidAt",
                table: "CreditPayments");

            migrationBuilder.DropColumn(
                name: "PlannedDateOfPayment",
                table: "CreditPayments");

            migrationBuilder.AddColumn<decimal>(
                name: "SinglePaymentAmount",
                table: "Credits",
                type: "decimal(10, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "CreditPayments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SinglePaymentAmount",
                table: "Credits");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "CreditPayments");

            migrationBuilder.AddColumn<int>(
                name: "PaymentDayOfMonth",
                table: "Credits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "CreditPayments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidAt",
                table: "CreditPayments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedDateOfPayment",
                table: "CreditPayments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
