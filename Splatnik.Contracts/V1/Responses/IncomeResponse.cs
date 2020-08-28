using System;

namespace Splatnik.Contracts.V1.Responses
{
	public class IncomeResponse
	{
		public int Id { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime IncomeDate { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public decimal IncomeValue { get; set; }
	}
}