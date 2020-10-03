using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.Dtos
{
    public class DebtDto
    {
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal InitialValue { get; set; }
        public int CurrencyId { get; set; }
        public int BudgetId { get; set; }
    }
}
