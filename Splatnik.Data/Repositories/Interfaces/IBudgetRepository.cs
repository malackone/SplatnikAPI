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
		Task<Period> GetPeriodAsync(int periodId);
		Task<IList<Period>> GetBudgetPeriodsAsync(int budgetId);
        Task<Period> GetCurrentPeriodAsync(int budgetId, DateTime today);
    }
}
