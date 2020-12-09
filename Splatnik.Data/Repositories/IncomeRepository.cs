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
	public class IncomeRepository : IIncomeRepository
	{
		private readonly DataContext _dataContext;

		public IncomeRepository(DataContext dataContext)
		{
			_dataContext = dataContext;
		}

		public async Task<IList<Income>> GetIncomesForPeriodAsync(int periodId)
		{
			return await _dataContext.Incomes.Where(x => x.PeriodId == periodId).ToListAsync();
		}

	}
}
