using Microsoft.EntityFrameworkCore;
using Splatnik.Data.Database;
using Splatnik.Data.Database.DbModels;
using Splatnik.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatnik.Data.Repositories
{
	public class BudgetRepository : IBudgetRepository
	{
		private readonly DataContext _dataContext;

		public BudgetRepository(DataContext dataContext)
		{
			_dataContext = dataContext;
		}

		public async Task<Budget> CreateBudgetAsync(Budget budget)
		{

			_dataContext.Budgets.Add(budget);
			await _dataContext.SaveChangesAsync();

			return budget;
		}

		public async Task<Budget> GetBudgetAsync(int budgetId)
		{
			return await _dataContext.Budgets
				.Include(x => x.Periods)
				.ThenInclude(x=>x.Incomes)
				.Include(x => x.Periods)
				.ThenInclude(x => x.Expenses)
				.FirstOrDefaultAsync(x => x.Id == budgetId);
		}

		public async Task<IList<Budget>> GetUserBudgets(string userId)
		{
			return await _dataContext.Budgets.Where(x => x.UserId == userId).ToListAsync();
		}

    }
}
