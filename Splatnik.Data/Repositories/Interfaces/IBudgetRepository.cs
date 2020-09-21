using Splatnik.Data.Database.DbModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Splatnik.Data.Repositories.Interfaces
{
	public interface IBudgetRepository
	{
		Task<Budget> CreateBudgetAsync(Budget budget);
		Task<Budget> GetBudgetAsync(int budgetId);
		Task<IList<Budget>> GetUserBudgets(string userId);
        
        Task<Period> CreatePeriodAsync(Period period);
		Task<Period> GetPeriodAsync(int budgetId, int periodId);
		Task<IList<Period>> GetBudgetPeriodsAsync(int budgetId);
        Task<Period> GetCurrentPeriodAsync(int budgetId, DateTime today);
        
        Task<Expense> CreateExpenseAsync(Expense expense);
        Task<Expense> GetExpenseAsync(int periodId, int expenseId);
        Task<IList<Expense>> GetExpensesAsync(int periodId);
        Task<bool> UpdateExpenseAsync(Expense expense);
        Task<bool> DeleteExepenseAsync(Expense expense);

        Task<Income> CreateIncomeAsync(Income income);
        Task<Income> GetIncomeAsync(int periodId, int incomeId);
        Task<IList<Income>> GetIncomesAsync(int periodId);
        Task<bool> UpdateIncomeAsync(Income income);
        Task<bool> DeleteIncomeAsync(Income income);
    }
}
