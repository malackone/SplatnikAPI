using System;
using System.Collections.Generic;

namespace Splatnik.Contracts.V1.Responses
{
	public class PeriodResponse
	{
		public int Id { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime FirstDay { get; set; }

		public DateTime LastDay { get; set; }

		public string DisplayName { get; set; }

		public string Notes { get; set; }

		public List<IncomeResponse> Incomes { get; set; }
		public List<ExpenseResponse> Expenses { get; set; }
		public List<DebtPaymentResponse> DebtPayments { get; set; }
		public List<CreditPaymentResponse> CreditPayments { get; set; }
	}
}