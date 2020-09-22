using System;

namespace Splatnik.Contracts.V1.Responses
{
    public class DebtPaymentResponse
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        public decimal DebtPaymentValue { get; set; }
        public int DebtId { get; set; }
        public int CurrencyId { get; set; }
        public int PeriodId { get; set; }

    }
}