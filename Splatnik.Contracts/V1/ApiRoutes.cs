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
		}

		public static class UserSetting
        {

        }
	}
}
