using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.Dtos
{
    public class UpdateDebtPaymentDto
    {
        public DateTime UpdatedAt { get; set; }
        public string Description { get; set; }
        public decimal DebtPaymentValue { get; set; }
        public int CurrencyId { get; set; }
        public int PeriodId { get; set; }
    }
}
