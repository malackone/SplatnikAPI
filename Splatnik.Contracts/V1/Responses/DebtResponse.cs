using System;
using System.Collections.Generic;
using System.Text;

namespace Splatnik.Contracts.V1.Responses
{
	public class DebtResponse
	{
		public int Id { get; set; }
		public DateTime CreatedAt { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal InitialValue { get; set; }
		public int BudgetId { get; set; }
		public int CurrencyId { get; set; }
		public virtual List<DebtPaymentResponse> DebtPayments { get; set; }
	}
}
