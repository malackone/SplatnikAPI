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
        #region Budgets
        Task<Budget> NewBudgetAsync(BudgetRequest budgetRequest, string userId);
		Task<Budget> GetBudgetAsync(int budgetId);
		Task<IList<Budget>> GetUserBudgets(string userId);
        #endregion

        #region Periods
        Task<Period> NewPeriodAsync(PeriodRequest request, int budgetId);
        Task<Period> GetPeriodAsync(int budgetId, int periodId);
        Task<Period> GetCurrentPeriodAsync(int budgetId, DateTime today);
        Task<IList<Period>> GetBudgetPeriodsAsync(int budgetId);
        #endregion

        #region Expenses
        Task<Expense> NewExpenseAsync(int periodId, ExpenseRequest request);
        Task<Expense> GetExpenseAsync(int periodId, int expenseId);
        Task<IList<Expense>> GetExpensesAsync(int periodId);
        Task<bool> UpdateExpenseAsync(int expenseId, UpdateExpenseRequest request);
        Task<bool> DeleteExpenseAsync(Expense expense);
        #endregion

        #region Incomes
        Task<Income> NewIncomeAsync(int periodID, IncomeRequest request);
        Task<Income> GetIncomeAsync(int periodId, int incomeId);
        Task<IList<Income>> GetIncomesAsync(int periodId);
        Task<bool> UpdateIncomeAsync(int incomeId, UpdateIncomeRequest request);
        Task<bool> DeleteIncomeAsync(Income income);
        #endregion


        #region Debts
        Task<Debt> NewDebtAsync(DebtRequest request, int budgetId);
        Task<Debt> GetDebtAsync(int budgetId, int debtId);
        Task<IList<Debt>> GetDebtsAsync(int budgetId);
        Task<bool> UpdateDebtAsync(int debtId, UpdateDebtRequest request);
        Task<bool> DeleteDebtAsync(Debt debt);
        #endregion
    }
}
