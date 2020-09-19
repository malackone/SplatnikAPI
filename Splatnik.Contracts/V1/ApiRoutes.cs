using System;
using System.Collections.Generic;
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
			public const string NewBudget = Base + "/user/budgets";
			public const string Budget = Base + "/user/budgets/{budgetId}";
			public const string Budgets = Base + "/user/budgets";

			public const string NewBudgetPeriod = Base + "/user/budgets/{budgetId}/periods";
			public const string BudgetPeriods = Base + "/user/budgets/{budgetId}/periods";
			public const string BudgetPeriod = Base + "/user/budgets/{budgetId}/periods/{periodId}";

			public const string NewExpense = Base + "/user/budgets/{budgetId}/periods/{periodId}/expenses";
			public const string BudgetPeriodExpense = Base + "/user/budgets/{budgetId}/periods/{periodId}/expenses/{expenseId}";

			public const string NewIncome = Base + "/user/budgets/{budgetId}/periods/{periodId}/incomes";
			public const string BudgetPeriodIncome = Base + "/user/budgets/{budgetId}/periods/{periodId}/incomes/{incomeId}";

		}
	}
}
