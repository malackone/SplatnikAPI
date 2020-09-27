using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Splatnik.Contracts.V1
{
	public class ApiRoutes
	{
		public const string Root = "api";
		public const string Version = "v1";
		public const string Base = Root + "/" + Version;

		public static class Identity
		{
			public const string Login = Base + "/identity/login";
			public const string Register = Base + "/identity/register";
			public const string Refresh = Base + "/identity/refresh";
			//public const string ConfirmEmail = Base + "/identity/confirmEmail?token={jwtToken}&email={email}";
			public const string ConfirmEmail = Base + "/identity/emailConfirmation";
		}

		public static class Admin
		{
			public const string Role = Base + "/admin/role";
			public const string UserRoles = Base + "/admin/userRoles";
			public const string TestEmail = Base + "/admin/testEmail";
		}


		public static class UserBudget
		{
			public const string CreateBudget = Base + "/user/budgets";
			public const string Budget = Base + "/user/budgets/{budgetId}";
			public const string Budgets = Base + "/user/budgets";

			public const string CreateBudgetPeriod = "/user/budgets/{budgetId}/periods";
			public const string BudgetPeriods = "/user/budgets/{budgetId}/periods";
			public const string BudgetPeriod = "/user/budgets/{budgetId}/periods/{periodId}";

			public const string CreateExpense = "/user/budgets/{budgetId}/periods/{periodId}/expenses";
			public const string BudgetPeriodExpenses = "/user/budgets/{budgetId}/periods/{periodId}/expenses";
			public const string BudgetPeriodExpense = "/user/budgets/{budgetId}/periods/{periodId}/expenses/{expenseId}";
			public const string UpdateExpense = "/user/budgets/{budgetId}/periods/{periodId}/expenses/{expenseId}";
			public const string DeleteExpense = "/user/budgets/{budgetId}/periods/{periodId}/expenses/{expenseId}";

			public const string CreateIncome = "/user/budgets/{budgetId}/periods/{periodId}/incomes";
			public const string BudgetPeriodIncomes = "/user/budgets/{budgetId}/periods/{periodId}incomes";
			public const string BudgetPeriodIncome = "/user/budgets/{budgetId}/periods/{periodId}incomes/{incomeId}";
			public const string UpdateIncome = "/user/budgets/{budgetId}/periods/{periodId}incomes/{incomeId}";
			public const string DeleteIncome = "/user/budgets/{budgetId}/periods/{periodId}/incomes/{incomeId}";

			public const string CreateDebt = "/user/budgets/{budgetId}/debts";
			public const string BudgetDebts = "/user/budgets/{budgetId}/debts";
			public const string BudgetDebt = "/user/budgets/{budgetId}/debts/{debtId}";
			public const string UpdateBudgetDebt = "/user/budgets/{budgetId}/debts/{debtId}";
			public const string DeleteBudgetDebt = "/user/budgets/{budgetId}/debts/{debtId}";

			public const string CreateDebtPayment = "/user/budgets/{budgetId}/debts/{debtId}/debtpayments";
			public const string BudgetDebtPayments = "/user/budgets/{budgetId}/debts/{debtId}/debtpayments";
			public const string BudgetDebtPayment = "/user/budgets/{budgetId}/debts/{debtId}/debtpayments/{debtPaymentId}";
			public const string UpdateBudgetDebtPayment = "/user/budgets/{budgetId}/debts/{debtId}/debtpayments/{debtPaymentId}";
			public const string DeleteBudgetDebtPayment = "/user/budgets/{budgetId}/debts/{debtId}/debtpayments/{debtPaymentId}";

			public const string CreateCredit = "/user/budgets/{budgetId}/credits";
			public const string BudgetCredits = "/user/budgets/{budgetId}/credits";
			public const string BudgetCredit = "/user/budgets/{budgetId}credits/{creditId}";
			public const string UpdateBudgetCredit = "/user/budgets/{budgetId}/credits/{creditId}";
			public const string DeleteBudgetCredit = "/user/budgets/{budgetId}credits/{creditId}";

			public const string CreateCreditPayment = "/user/budgets/{budgetId}credits/{creditId}/creditpayments";
			public const string BudgetCreditPayments = "/user/budgets/{budgetId}credits/{creditId}/creditpayments";
			public const string BudgetCreditPayment = "/user/budgets/{budgetId}credits/{creditId}/creditpayments/{creditPaymentId}";
			public const string UpdateBudgetCreditPayment = "/user/budgets/{budgetId}credits/{creditId}/creditpayments/{creditPaymentId}";
			public const string DeleteBudgetCreditPayment = "/user/budgets/{budgetId}credits/{creditId}/creditpayments/{creditPaymentId}";


		}
	}

		public static class UserSettings
        {

        }
}
