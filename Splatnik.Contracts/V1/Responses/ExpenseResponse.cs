using System;

namespace Splatnik.Contracts.V1.Responses
{
	public class ExpenseResponse
	{
		public int Id { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime IncomeDate { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public decimal ExpanseValue { get; set; }

	}
}