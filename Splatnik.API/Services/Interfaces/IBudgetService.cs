using Splatnik.Contracts.V1.Requests;
using Splatnik.Data.Database.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.Services.Interfaces
{
	public interface IBudgetService
	{
		Task<Budget> NewBudgetAsync(BudgetRequest budgetRequest, string userId);
		Task<Budget> GetBudgetAsync(int budgetId);
		Task<IList<Budget>> GetUserBudgets(string userId);

        Task<Period> NewPeriodAsync(PeriodRequest request, int budgetId);
        Task<Period> GetPeriodAsync(int budgetId, int periodId);
        Task<Period> GetCurrentPeriodAsync(int budgetId, DateTime today);
        Task<IList<Period>> GetBudgetPeriodsAsync(int budgetId);

        Task<Expense> NewExpenseAsync(int periodId, ExpenseRequest request);
        Task<Expense> GetExpenseAsync(int periodId, int expenseId);
        Task<bool> UpdateExpenseAsync(int expenseId, UpdateExpenseRequest request);

        Task<Income> NewIncomeAsync(int periodID, IncomeRequest request);
        Task<Income> GetIncomeAsync(int periodId, int incomeId);
        Task<bool> UpdateIncomeAsync(int incomeId, UpdateIncomeRequest request);
    }
}
