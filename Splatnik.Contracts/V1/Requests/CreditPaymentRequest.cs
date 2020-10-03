using System;
using System.Collections.Generic;
using System.Text;

namespace Splatnik.Contracts.V1.Requests
{
    public class CreditPaymentRequest
    {
        public string UserId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal PaymentValue { get; set; }
        public int PeriodId { get; set; }
        public int CurrencyId { get; set; }

    }
}
