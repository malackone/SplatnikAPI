using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.Dtos
{
	public class CreditDto
	{
		public DateTime CreatedAt { get; set; }

		public string UserId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string BankName { get; set; }
		public string BankAccountNumber { get; set; }
		public string ContractNumber { get; set; }
		public int InitNoOfPayments { get; set; }
		public decimal InitialCreditValue { get; set; }
		public int CurrencyId { get; set; }
		public int BudgetId { get; set; }
		//public virtual List<CreditPayment> CreditPayments { get; set; }
	}
}
