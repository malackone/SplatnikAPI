using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.Dtos
{
	public class PeriodDto
	{
		public string UserId { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime FirstDay { get; set; }
		public DateTime LastDay { get; set; }
		public string DisplayName { get; set; }
		public string Notes { get; set; }
		public int BudgetId { get; set; }
	}
}
