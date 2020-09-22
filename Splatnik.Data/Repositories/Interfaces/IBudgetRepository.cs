using Splatnik.Data.Database.DbModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Splatnik.Data.Repositories.Interfaces
{
	public interface IBudgetRepository
	{
        #region Budgets
        Task<Budget> CreateBudgetAsync(Budget budget);
		Task<Budget> GetBudgetAsync(int budgetId);
		Task<IList<Budget>> GetUserBudgets(string userId);
        #endregion

        #region Periods
        Task<Period> CreatePeriodAsync(Period period);
		Task<Period> GetPeriodAsync(int budgetId, int periodId);
		Task<IList<Period>> GetBudgetPeriodsAsync(int budgetId);
        Task<Period> GetCurrentPeriodAsync(int budgetId, DateTime today);
        #endregion

        #region Expenses
        Task<Expense> CreateExpenseAsync(Expense expense);
        Task<Expense> GetExpenseAsync(int periodId, int expenseId);
        Task<IList<Expense>> GetExpensesAsync(int periodId);
        Task<bool> UpdateExpenseAsync(Expense expense);
        Task<bool> DeleteExepenseAsync(Expense expense);
        #endregion

        #region Incomes
        Task<Income> CreateIncomeAsync(Income income);
        Task<Income> GetIncomeAsync(int periodId, int incomeId);
        Task<IList<Income>> GetIncomesAsync(int periodId);
        Task<bool> UpdateIncomeAsync(Income income);
        Task<bool> DeleteIncomeAsync(Income income);
        #endregion

        #region Debts
        Task<Debt> CreateDebtAsync(Debt debt);
        Task<Debt> GetDebtAsync(int budgetId, int debtId);
        Task<IList<Debt>> GetDebtsAsync(int budgetId);
        Task<bool> UpdateDebtAsync(Debt debt);
        Task<bool> DeleteDebtAsync(Debt debt);
        #endregion
    }
}
