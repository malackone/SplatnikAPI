using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.Dtos
{
    public class ExpenseDto
    {
		public DateTime CreatedAt { get; set; }
		public DateTime IncomeDate { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal ExpanseValue { get; set; }
		public int CurrencyId { get; set; }

		public int PeriodId { get; set; }
	}
}
