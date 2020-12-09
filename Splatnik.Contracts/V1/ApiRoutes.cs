using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
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
		}

		public static class UserIncomes
		{
			public const string CreateIncome = "/user/incomes";
			public const string BudgetPeriodIncomes = "/user/incomes";
			public const string BudgetPeriodIncome = "/user/incomes/{incomeId}";
			public const string UpdateIncome = "/user/incomes/{incomeId}";
			public const string DeleteIncome = "/user/incomes/{incomeId}";
		}

		public static class UserExpenses
		{
			public const string CreateExpense = "/user/expenses";
			public const string BudgetPeriodExpenses = "/user/expenses";
			public const string BudgetPeriodExpense = "/user/expenses/{expenseId}";
			public const string UpdateExpense = "/user/expenses/{expenseId}";
			public const string DeleteExpense = "/user/expenses/{expenseId}";
		}

		public static class UserCredits
		{
			public const string CreateCredit = "/user/credits";
			public const string BudgetCredits = "/user/credits";
			public const string BudgetCredit = "/user/credits/{creditId}";
			public const string UpdateBudgetCredit = "/user/credits/{creditId}";
			public const string DeleteBudgetCredit = "/user/credits/{creditId}";

			public const string CreateCreditPayment = "/user/credits/{creditId}/creditpayments";
			public const string BudgetCreditPayments = "/user/credits/{creditId}/creditpayments";
			public const string BudgetCreditPayment = "/user/credits/{creditId}/creditpayments/{creditPaymentId}";
			public const string UpdateBudgetCreditPayment = "/user/credits/{creditId}/creditpayments/{creditPaymentId}";
			public const string DeleteBudgetCreditPayment = "/user/credits/{creditId}/creditpayments/{creditPaymentId}";
		}

		public static class UserDebts
		{
			public const string CreateDebt = "/user/debts";
			public const string BudgetDebts = "/user/debts";
			public const string BudgetDebt = "/user/debts/{debtId}";
			public const string UpdateBudgetDebt = "/user/debts/{debtId}";
			public const string DeleteBudgetDebt = "/user/debts/{debtId}";

			public const string CreateDebtPayment = "/user/debts/{debtId}/debtpayments";
			public const string BudgetDebtPayments = "/user/debts/{debtId}/debtpayments";
			public const string BudgetDebtPayment = "/user/{debtId}/debtpayments/{debtPaymentId}";
			public const string UpdateBudgetDebtPayment = "/user/debts/{debtId}/debtpayments/{debtPaymentId}";
			public const string DeleteBudgetDebtPayment = "/user/debts/{debtId}/debtpayments/{debtPaymentId}";
		}

		public static class UserSettings
		{

		}
	}

}
