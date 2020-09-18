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
		Task<Budget> CreateBudgetAsync(NewBudgetRequest budgetRequest, string userId);
		Task<Budget> GetBudgetAsync(int budgetId);
		Task<IList<Budget>> GetUserBudgets(string userId);
        Task<Period> CreatePeriodAsync(NewPeriodRequest request, string userId);
        Task<Period> GetPeriodAsync(int periodId);
        Task<Period> GetCurrentPeriodAsync(int budgetId, DateTime today);
        Task<IList<Period>> GetBudgetPeriodsAsync(int budgetId);
    }
}
