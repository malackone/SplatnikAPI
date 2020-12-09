using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.Dtos
{
	public class UpdateCreditPaymentDto
	{
		public int Id { get; set; }
		public DateTime PaymentDate { get; set; }
		public decimal PaymentValue { get; set; }
	}
}
