using System;
using System.Collections.Generic;
using System.Text;

namespace Splatnik.Contracts.V1.Requests
{
    public class NewExpenseRequest
    {
		public DateTime IncomeDate { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public decimal ExpanseValue { get; set; }
		public int CurrencyId { get; set; }

	}
}
