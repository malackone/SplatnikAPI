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
	public class PeriodRepository : IPeriodRepository
	{
		private readonly DataContext _dataContext;

		public PeriodRepository(DataContext dataContext)
		{
			_dataContext = dataContext;
		}

		public async Task<IList<Period>> GetBudgetPeriodsAsync(int budgetId)
		{
			return await _dataContext.Periods.Where(x => x.BudgetId == budgetId).ToListAsync();
		}

	}
}
