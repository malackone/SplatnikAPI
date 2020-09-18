using System;
using System.Collections.Generic;
using System.Text;

namespace Splatnik.Contracts.V1.Requests
{
    public class NewPeriodRequest
    {
		public DateTime FirstDay { get; set; }
		public DateTime LastDay { get; set; }
		public string DisplayName { get; set; }
		public string Notes { get; set; }
		public int BudgetId { get; set; }
	}
}
