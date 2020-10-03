using System;
using System.Collections.Generic;
using System.Text;

namespace Splatnik.Contracts.V1.Requests
{
    public class ExpenseRequest
    {
		public DateTime ExpenseDate { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public decimal ExpenseValue { get; set; }
		public int CurrencyId { get; set; }
        public int PeriodId { get; set; }
        public int BudgetId { get; set; }

    }
}
