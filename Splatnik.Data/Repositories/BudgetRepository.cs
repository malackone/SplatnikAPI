using Splatnik.Data.Database;
using Splatnik.Data.Database.DbModels;
using Splatnik.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
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

		public async Task<Budget> CreateBudgetAsync()
		{
			var budget = new Budget
			{
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow,
				Name = "Test",
				Description = "Test description",
				UserId = "312160ef-44f3-42f3-8b7b-9dc8f9bd161e",
			};

			_dataContext.Budgets.Add(budget);
			await _dataContext.SaveChangesAsync();

			return budget;
		}

		public Task<Budget> GetBudgetAsync(int budgetId)
		{
			throw new NotImplementedException();
		}

		public Task<IList<Budget>> GetUserBudgets(string userId)
		{
			throw new NotImplementedException();
		}
	}
}
