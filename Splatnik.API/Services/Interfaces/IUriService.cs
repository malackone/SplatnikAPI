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
		Uri GetExpenseUri(int expenseId);
        Uri GetIncomeUri(int incomeId);
        Uri GetDebtUri(int debtId);
        Uri GetDebtPaymentUri(int debtId, int debtPaymentId);
        Uri GetCreditUri(int id);
        Uri GetCreditPaymentUri(int creditId, int creditPaymentId);
    }
}
