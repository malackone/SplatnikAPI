using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.Dtos
{
	public class CreditPaymentDto
	{
		public DateTime CreatedDate { get; set; }
		public DateTime PaymentDate { get; set; }
		public decimal PaymentValue { get; set; }
		public int CurrencyId { get; set; }
		public int PeriodId { get; set; }
		public int CreditId { get; set; }
		public string UserId { get; set; }
	}
}
