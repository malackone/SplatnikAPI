using Splatnik.API.Services.Interfaces;
using Splatnik.Contracts.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.Services
{
	public class UriService : IUriService
	{
		private readonly string _baseUri;

		public UriService(string baseUri)
		{
			_baseUri = baseUri;
		}

		public Uri GetBudgetUri(int budgetId)
		{
			return new Uri(_baseUri + ApiRoutes.UserBudget.Budget
				.Replace("{budgetId}", budgetId.ToString()));
		}

		public Uri GetPeriodUri(int budgetId, int periodId)
        {
			return new Uri(_baseUri + ApiRoutes.UserBudget.BudgetPeriod
				.Replace("{budgetId}", budgetId.ToString())
				.Replace("{periodId}", periodId.ToString()));
        }

		public Uri GetConfirmationLink(string email, string token)
		{
			return new Uri(_baseUri + ApiRoutes.Identity.ConfirmEmail + "/" + email + "/" + token);
		}

        public Uri GetExpenseUri(int budgetId, int periodId, int expenseId)
        {
			return new Uri(_baseUri + ApiRoutes.UserBudget.BudgetPeriodExpense
				.Replace("{budgetId}", budgetId.ToString())
				.Replace("{periodId}", periodId.ToString())
				.Replace("{expenseId}", expenseId.ToString()));
		}

        public Uri GetIncomeUri(int budgetId, int periodId, int incomeId)
        {
			return new Uri(_baseUri + ApiRoutes.UserBudget.BudgetPeriodExpense
				.Replace("{budgetId}", budgetId.ToString())
				.Replace("{periodId}", periodId.ToString())
				.Replace("{incomeId}", incomeId.ToString()));
		}

        public Uri GetDebtUri(int budgetId, int debtId)
        {
            return new Uri(_baseUri + ApiRoutes.UserBudget.BudgetDebt
				.Replace("{budgetId}", budgetId.ToString())
				.Replace("{debtId}", debtId.ToString()));
		}

        public Uri GetDebtPaymentUri(int budgetId, int debtPaymentId)
        {
			return new Uri(_baseUri + ApiRoutes.UserBudget.BudgetDebtPayments
				.Replace("{budgetId}", budgetId.ToString())
				.Replace("{debtPaymentId}", debtPaymentId.ToString()));
		}
    }
}
