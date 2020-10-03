using System;
using System.Collections.Generic;
using System.Text;

namespace Splatnik.Contracts.V1.Requests
{
    public class UpdateCreditPaymentRequest
    {
        public DateTime PaymentDate { get; set; }
        public decimal PaymentValue { get; set; }
    }
}
