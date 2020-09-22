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

			public const string CreateBudgetPeriod = Budget + "/periods";
			public const string BudgetPeriods = Budget + "/periods";
			public const string BudgetPeriod = Budget + "/periods/{periodId}";

			public const string CreateExpense = BudgetPeriod + "/expenses";
			public const string BudgetPeriodExpenses = BudgetPeriod + "/expenses";
			public const string BudgetPeriodExpense = BudgetPeriod + "/expenses/{expenseId}";
			public const string UpdateExpense = BudgetPeriod + "/expenses/{expenseId}";
			public const string DeleteExpense = BudgetPeriod + "/expenses/{expenseId}";

			public const string CreateIncome = BudgetPeriod + "/incomes";
			public const string BudgetPeriodIncomes = BudgetPeriod + "/incomes";
			public const string BudgetPeriodIncome = BudgetPeriod + "/incomes/{incomeId}";
			public const string UpdateIncome = BudgetPeriod + "/incomes/{incomeId}";
			public const string DeleteIncome = BudgetPeriod + "/incomes/{incomeId}";

			public const string CreateDebt = Budget + "/debts";
			public const string BudgetDebts = Budget + "/debts";
			public const string BudgetDebt = Budget + "/debts/{debtId}";
			public const string UpdateBudgetDebt = Budget + "/debts/{debtId}";
			public const string DeleteBudgetDebt = Budget + "/debts/{debtId}";

			public const string CreateDebtPayment = BudgetDebt + "/debtpayments";
			public const string BudgetDebtPayments = BudgetDebt + "/debtpayments";
			public const string BudgetDebtPayment = BudgetDebt + "/debtpayments/{debtPeymentId}";
			public const string UpdateBudgetDebtPayment = BudgetDebt + "/debtpayments/{debtPeymentId}";
			public const string DeleteBudgetDebtPayment = BudgetDebt + "/debtpayments/{debtPeymentId}";

			public const string CreateCredit = Budget + "/credits";
			public const string BudgetCredits = Budget + "/credits";
			public const string BudgetCredit = Budget + "/credits/{creditId}";
			public const string UpdateBudgetCredit = Budget + "/credits/{creditId}";
			public const string DeleteBudgetCredit = Budget + "/credits/{creditId}";

			public const string CreateCreditPayment = BudgetCredit + "/creditpayments";
			public const string BudgetCreditPayments = BudgetCredit + "/creditpayments";
			public const string BudgetCreditPayment = BudgetCredit + "/creditpayments/{creditPeymentId}";
			public const string UpdateBudgetCreditPayment = BudgetCredit + "/creditpayments/{creditPeymentId}";
			public const string DeleteBudgetCreditPayment = BudgetCredit + "/creditpayments/{creditPeymentId}";


		}
	}

		public static class UserSettings
        {

        }
}
