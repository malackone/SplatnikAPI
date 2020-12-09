using System;
using System.Collections.Generic;
using System.Text;

namespace Splatnik.Contracts.V1.Requests
{
	public class UpdateDebtPaymentRequest
	{
		public string Description { get; set; }
		public decimal DebtPaymentValue { get; set; }
		public int CurrencyId { get; set; }
		public int PeriodId { get; set; }
		public int BudgetId { get; set; }
	}
}
