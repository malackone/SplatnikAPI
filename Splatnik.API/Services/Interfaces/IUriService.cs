using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.Services.Interfaces
{
	public interface IUriService
	{
		Uri GetBudgetUri(int budgetId);
		Uri GetPeriodUri(int budgetId, int periodId);
		Uri GetConfirmationLink(string email, string token);
		Uri GetExpenseUri(int budgetId, int periodId, int expenseId);
        Uri GetIncomeUri(int budgetId, int periodId, int incomeId);
        Uri GetDebtUri(int budgetId, int debtId);
        Uri GetDebtPaymentUri(int budgetId, int debtPaymentId);
    }
}
