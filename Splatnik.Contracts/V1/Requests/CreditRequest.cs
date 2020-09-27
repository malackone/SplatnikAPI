using System;
using System.Collections.Generic;
using System.Text;

namespace Splatnik.Contracts.V1.Requests
{
    public class CreditRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string ContractNumber { get; set; }
        public int InitNoOfPayments { get; set; }
        public decimal InitialCreditValue { get; set; }
        public int PaymentDayOfMonth { get; set; }
        public int CurrencyId { get; set; }
        public int BudgetId { get; set; }
    }
}
