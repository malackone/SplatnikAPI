using Splatnik.Contracts.V1.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Splatnik.Contracts.V1.Responses
{
	public class BudgetResponse
	{
		public int Id { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public List<PeriodResponse> Periods { get; set; }
	}
}
